namespace DomainPlayground.Core.Application.Models.IdentityModels;

public record TokenResponseDto(
    string AccessToken,
    string RefreshToken,
    DateTime RefreshTokenExpiryTime);