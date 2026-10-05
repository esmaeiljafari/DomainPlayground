namespace DomainPlayground.Infrastructure.Persistence.Queries.Authorization.ReadModels;

public sealed class UserPermissionOverrideReadModel
{
    public Guid UserAccessId { get; set; }
    public int PermissionId { get; set; }
    public int Effect { get; set; } 
}