namespace DomainPlayground.Infrastructure.Persistence.Queries.Authorization.ReadModels;

public sealed class UserAccessRoleReadModel
{
    public Guid UserAccessId { get; set; }
    public int RoleId { get; set; }
}