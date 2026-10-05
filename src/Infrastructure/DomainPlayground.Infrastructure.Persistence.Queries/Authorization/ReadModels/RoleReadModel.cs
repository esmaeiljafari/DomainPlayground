namespace DomainPlayground.Infrastructure.Persistence.Queries.Authorization.ReadModels;

public sealed class RoleReadModel
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public bool IsSystem { get; set; }
    public bool IsActive { get; set; }
}