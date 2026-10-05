namespace DomainPlayground.Infrastructure.Persistence.Queries.Authorization.ReadModels;

public sealed class RolePermissionReadModel
{
    public int RoleId { get; set; }
    public int PermissionId { get; set; }
}