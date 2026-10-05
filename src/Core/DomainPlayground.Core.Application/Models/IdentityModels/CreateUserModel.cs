namespace DomainPlayground.Core.Application.Models.IdentityModels;

public record CreateUserModel(
    string Email,
    string Password,
    string UserName,
    string FirstName,
    string LastName,
    Guid DomainUserId);
