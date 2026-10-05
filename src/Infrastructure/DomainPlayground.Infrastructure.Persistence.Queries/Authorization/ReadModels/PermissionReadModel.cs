namespace DomainPlayground.Infrastructure.Persistence.Queries.Authorization.ReadModels;

public sealed class PermissionReadModel
{
    public int Id { get; set; }
    public string Key { get; set; } = default!;
    public string Title { get; set; } = default!;
}