namespace DomainPlayground.Core.Application.Models.IdentityModels;

public record TokenRequestDto(
    string AccessToken,
    string RefreshToken);
