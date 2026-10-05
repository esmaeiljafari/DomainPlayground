using DomainPlayground.Core.Application.Common.Abstractions.Security;
using DomainPlayground.Core.Domain.Authorization.Permissions;
using DomainPlayground.Core.Domain.Authorization.Permissions.ValueObjects;
using DomainPlayground.Infrastructure.Persistence.Commands.Common.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Reflection;

namespace DomainPlayground.Infrastructure.Persistence.Commands.Authorization.Services;

public sealed class PermissionSynchronizer(ApplicationCommandDbContext db, ILogger<PermissionSynchronizer> logger)
{
    public async Task SyncAsync(Assembly applicationAssembly, CancellationToken ct = default)
    {
        var discovered = applicationAssembly.GetTypes()
            .Select(t => t.GetCustomAttribute<HasPermissionAttribute>())
            .Where(a => a is not null)
            .Select(a => a!)
            .GroupBy(a => $"{a.ModuleCode.ToLowerInvariant()}.{a.PermissionCode}")
            .Select(g => g.First())
            .ToList();

        var codeKeys = discovered
            .Select(a => $"{a.ModuleCode.ToLowerInvariant()}.{a.PermissionCode.ToLowerInvariant()}")
            .ToHashSet();

        var dbPermissions = await db.Permissions.ToListAsync(ct);

        foreach (var attr in discovered)
        {
            var fullKey = $"{attr.ModuleCode.ToLowerInvariant()}.{attr.PermissionCode.ToLowerInvariant()}";
            var existing = dbPermissions.FirstOrDefault(p => p.Key.Value == fullKey);

            if (existing is null)
            {
                var keyResult = PermissionKey.Create(fullKey);
                if (keyResult.IsFailure)
                {
                    logger.LogWarning("Permission key '{Key}' is invalid and was skipped: {Error}", fullKey, keyResult.Error.Code);
                    continue;
                }

                var permissionResult = Permission.Register(keyResult.Value, attr.PermissionTitle);
                if (permissionResult.IsSuccess)
                    db.Permissions.Add(permissionResult.Value);
            }
            else
            {
                existing.Rename(attr.PermissionTitle);
            }
        }

        var toRemove = dbPermissions.Where(p => !codeKeys.Contains(p.Key.Value)).ToList();
        if (toRemove.Count > 0)
        {
            var removeIds = toRemove.Select(p => p.Id.Value).ToList();
            var idsCsv = string.Join(",", removeIds);

            await db.Database.ExecuteSqlRawAsync($"DELETE FROM RolePermissions WHERE PermissionId IN ({idsCsv})", ct);
            await db.Database.ExecuteSqlRawAsync($"DELETE FROM UserAccessOverrides WHERE PermissionId IN ({idsCsv})", ct);

            db.Permissions.RemoveRange(toRemove);
        }

        await db.SaveChangesAsync(ct);
    }
}