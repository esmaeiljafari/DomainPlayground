using DomainPlayground.Core.Application.Features.Authorization.Roles.Queries.GetAllRoles;
using DomainPlayground.Core.Application.Features.Authorization.Roles.Queries.GetRolePermissions;
using DomainPlayground.Core.Application.Features.Authorization.UserAccesses.Queries.GetUserRoles;

namespace DomainPlayground.Core.Application.Features.Authorization.Interfaces;

public interface IAuthorizationQueries
{
    Task<List<RoleDto>> GetAllRolesAsync(CancellationToken ct = default);
    Task<List<UserRoleDto>> GetUserRolesAsync(Guid userId, CancellationToken ct = default);
    Task<HashSet<string>> GetUserPermissionKeysAsync(Guid userId, CancellationToken ct = default);
    Task<List<RolePermissionGroupDto>> GetRolePermissionsAsync(int roleId, CancellationToken ct = default);  

}