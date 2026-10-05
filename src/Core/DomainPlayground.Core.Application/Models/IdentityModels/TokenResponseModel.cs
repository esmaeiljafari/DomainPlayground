namespace DomainPlayground.Core.Application.Models.IdentityModels;

public record TokenResponseModel
{
    public required string AccessToken { get; init; }
    public required string RefreshToken { get; init; }
    public required DateTime ExpiresAt { get; init; }
}