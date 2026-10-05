using DomainPlayground.Core.Domain.Authorization.UserAccesses.ValueObjects;

public sealed record AuthenticatedUser(UserId UserId, string UserName);