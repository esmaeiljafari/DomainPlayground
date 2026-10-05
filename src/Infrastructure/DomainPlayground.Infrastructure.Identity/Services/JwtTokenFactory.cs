using DomainPlayground.Infrastructure.Identity.Settings;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DomainPlayground.Infrastructure.Identity.Services;

internal static class JwtTokenFactory
{
    public static (string Token, DateTime ExpiresAtUtc) CreateAccessToken(
     Guid userId, JwtSettings settings)
    {
        var now = DateTime.UtcNow;
        var expires = now.AddMinutes(settings.ExpiryMinutes);

        var claims = new[]
        {
        new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
    };

        var secretValue = settings.Secret;

        if (string.IsNullOrEmpty(secretValue))
            throw new ArgumentNullException(nameof(settings.Secret), "JWT Secret is not configured.");

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretValue));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var jwt = new JwtSecurityToken(
            issuer: settings.Issuer,
            audience: settings.Audience,
            claims: claims,
            notBefore: now,
            expires: expires,
            signingCredentials: creds);

        return (new JwtSecurityTokenHandler().WriteToken(jwt), expires);
    }
}