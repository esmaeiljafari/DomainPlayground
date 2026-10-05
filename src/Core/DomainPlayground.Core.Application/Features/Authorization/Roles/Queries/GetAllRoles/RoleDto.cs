namespace DomainPlayground.Core.Application.Features.Authorization.Roles.Queries.GetAllRoles;

public sealed record RoleDto(int Id, string Name, bool IsSystem, bool IsActive);
