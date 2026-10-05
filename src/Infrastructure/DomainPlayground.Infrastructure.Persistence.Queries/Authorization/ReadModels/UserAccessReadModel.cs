namespace DomainPlayground.Infrastructure.Persistence.Queries.Authorization.ReadModels;

public sealed class UserAccessReadModel
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
}