using DomainPlayground.Core.Application.Features.Identity.Interfaces;
using DomainPlayground.Core.Domain.Authorization.Roles;
using DomainPlayground.Core.Domain.Authorization.UserAccesses;
using DomainPlayground.Core.Domain.Authorization.UserAccesses.ValueObjects;
using DomainPlayground.Infrastructure.Persistence.Commands.Common.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Reflection;

namespace DomainPlayground.Infrastructure.Persistence.Commands.Authorization.Services;

public sealed class AuthorizationSeeder(
    ApplicationCommandDbContext db,
    PermissionSynchronizer permissionSynchronizer,
    IIdentityService identityService,
    IConfiguration configuration,
    ILogger<AuthorizationSeeder> logger)
{
    private const string AdminRoleName = "Admin";

    public async Task SeedAsync(Assembly applicationAssembly, CancellationToken ct = default)
    {
        logger.LogInformation("Authorization seed started.");

        await permissionSynchronizer.SyncAsync(applicationAssembly, ct);

        var adminRole = await db.Roles.FirstOrDefaultAsync(r => r.Name == AdminRoleName, ct);
        if (adminRole is null)
        {
            var created = Role.Create(AdminRoleName, isSystem: true);
            if (created.IsFailure)
            {
                logger.LogError("Failed to create Admin role: {Error}", created.Error.Code);
                return;
            }

            adminRole = created.Value;
            db.Roles.Add(adminRole);
            await db.SaveChangesAsync(ct);
            logger.LogInformation("Admin role created.");
        }

        var allPermissionIds = await db.Permissions.Select(p => p.Id).ToListAsync(ct);
        adminRole.SetPermissions(allPermissionIds);
        await db.SaveChangesAsync(ct);

        var adminUserName = configuration["SeedAdmin:UserName"] ?? "admin";
        var adminPassword = configuration["SeedAdmin:Password"];

        if (string.IsNullOrWhiteSpace(adminPassword))
        {
            logger.LogWarning("SeedAdmin:Password is not configured. Skipping admin user seed.");
            return;
        }

        var hasAdminAccess = await db.UserAccesses
            .AnyAsync(ua => ua.Roles.Any(r => r.RoleId == adminRole.Id), ct);

        var newUserId = new UserId(Guid.NewGuid());
        var createUserResult = await identityService.CreateUserAsync(newUserId, adminUserName, adminPassword, ct);

        if (createUserResult.IsSuccess)
        {
            var access = UserAccess.Create(newUserId);
            access.AssignRole(adminRole.Id);
            db.UserAccesses.Add(access);
            await db.SaveChangesAsync(ct);

            logger.LogInformation("Admin user '{UserName}' created and assigned to Admin role.", adminUserName);
        }
        else
        {
            logger.LogInformation(
                "Admin user not created (likely already exists): {Error}", createUserResult.Error.Code);
        }

        logger.LogInformation("Authorization seed finished.");
    }
}