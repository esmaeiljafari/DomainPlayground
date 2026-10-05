namespace DomainPlayground.Infrastructure.Persistence.Queries.Identity.ReadModels;

public sealed class UserReadModel
{
    public Guid Id { get; set; }
    public string UserName { get; set; } = default!;
}