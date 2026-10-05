using DomainPlayground.Core.Application.Features.Authorization.Interfaces;
using DomainPlayground.Core.Application.Features.Authorization.Roles.Queries.GetAllRoles;
using DomainPlayground.Core.Application.Features.Authorization.Roles.Queries.GetRolePermissions;
using DomainPlayground.Core.Application.Features.Authorization.UserAccesses.Queries.GetUserRoles;
using DomainPlayground.Infrastructure.Persistence.Queries.Contexts;
using DomainPlayground.SharedKernel.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace DomainPlayground.Infrastructure.Persistence.Queries.Authorization.Repositories;

public sealed class AuthorizationQueriesRepository(ApplicationQueryDbContext db) : IAuthorizationQueries
{
    public async Task<List<RoleDto>> GetAllRolesAsync(CancellationToken ct = default)
    {
        var permissionCounts = await db.RolePermissions
            .GroupBy(rp => rp.RoleId)
            .Select(g => new { RoleId = g.Key, Count = g.Count() })
            .ToDictionaryAsync(x => x.RoleId, x => x.Count, ct);

        var roles = await db.Roles.AsNoTracking().ToListAsync(ct);

        return roles
            .Select(r => new RoleDto(
                r.Id, r.Name, r.IsSystem, r.IsActive
                ))
            .ToList();
    }

    public async Task<List<UserRoleDto>> GetUserRolesAsync(Guid userId, CancellationToken ct = default)
    {
        var userAccess = await db.UserAccesses.FirstOrDefaultAsync(u => u.UserId == userId, ct);

        var assignedRoleIds = userAccess is null
            ? new HashSet<int>()
            : (await db.UserAccessRoles
                .Where(x => x.UserAccessId == userAccess.Id)
                .Select(x => x.RoleId)
                .ToListAsync(ct))
              .ToHashSet();

        var roles = await db.Roles.AsNoTracking().ToListAsync(ct);

        return roles
            .Select(r => new UserRoleDto(r.Id, r.Name, assignedRoleIds.Contains(r.Id)))
            .ToList();
    }

    public async Task<HashSet<string>> GetUserPermissionKeysAsync(Guid userId, CancellationToken ct = default)
    {
        var userAccess = await db.UserAccesses.FirstOrDefaultAsync(u => u.UserId == userId, ct);
        if (userAccess is null) return [];

        var roleIds = await db.UserAccessRoles
            .Where(x => x.UserAccessId == userAccess.Id)
            .Select(x => x.RoleId)
            .ToListAsync(ct);

        var activeRoleIds = await db.Roles
            .Where(r => roleIds.Contains(r.Id) && r.IsActive)
            .Select(r => r.Id)
            .ToListAsync(ct);

        var fromRoles = await (
            from rp in db.RolePermissions
            join p in db.Permissions on rp.PermissionId equals p.Id
            where activeRoleIds.Contains(rp.RoleId) 
            select p.Key
        ).ToListAsync(ct);

        var effective = fromRoles.ToHashSet();

        var overrides = await (
            from o in db.UserAccessOverrides
            join p in db.Permissions on o.PermissionId equals p.Id
            where o.UserAccessId == userAccess.Id
            select new { p.Key, o.Effect }
        ).ToListAsync(ct);

        foreach (var o in overrides)
        {
            if (o.Effect == 1) effective.Add(o.Key);
            else if (o.Effect == 2) effective.Remove(o.Key);
        }

        return effective;
    }

    public async Task<List<RolePermissionGroupDto>> GetRolePermissionsAsync(int roleId, CancellationToken ct = default)
    {
        var grantedSet = (await db.RolePermissions
            .Where(rp => rp.RoleId == roleId)
            .Select(rp => rp.PermissionId)
            .ToListAsync(ct))
            .ToHashSet();

        var allPermissions = await db.Permissions.AsNoTracking().ToListAsync(ct);

        return allPermissions
            .Select(p => new
            {
                Permission = p,
                ModuleCode = p.Key.Split('.')[0]  
            })
            .GroupBy(x => x.ModuleCode)
            .Select(g => new RolePermissionGroupDto(
                g.Key,
                ModuleCodes.GetTitle(g.Key),
                g.Select(x => new RolePermissionItemDto(
                    x.Permission.Id,
                    x.Permission.Key,
                    x.Permission.Title,
                    grantedSet.Contains(x.Permission.Id)))
                 .ToList()))
            .OrderBy(g => g.ModuleCode)
            .ToList();
    }
}