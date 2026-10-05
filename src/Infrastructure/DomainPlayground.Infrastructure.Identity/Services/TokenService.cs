using DomainPlayground.Core.Application.Features.Identity.Errors;
using DomainPlayground.Core.Application.Features.Identity.Interfaces;
using DomainPlayground.Core.Application.Features.Identity.Models;
using DomainPlayground.Core.Domain.Authorization.UserAccesses.ValueObjects;
using DomainPlayground.Infrastructure.Identity.Contexts;
using DomainPlayground.Infrastructure.Identity.Models;
using DomainPlayground.Infrastructure.Identity.Settings;
using DomainPlayground.SharedKernel.Results;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace DomainPlayground.Infrastructure.Identity.Services;

public sealed class TokenService(IdentityAppDbContext db, IOptions<JwtSettings> jwtOptions) : ITokenService
{
    private readonly JwtSettings _settings = jwtOptions.Value;

    public async Task<TokenPair> IssueAsync(UserId userId, CancellationToken ct = default)
    {
        var pair = BuildTokenPair(userId.Value);

        db.RefreshTokens.Add(RefreshToken.Create(
            userId.Value, Hash(pair.RefreshToken), pair.RefreshTokenExpiresAtUtc));

        await db.SaveChangesAsync(ct);
        return pair;
    }

    public async Task<Result<TokenPair>> RefreshAsync(string refreshToken, CancellationToken ct = default)
    {
        var hash = Hash(refreshToken);
        var existing = await db.RefreshTokens.FirstOrDefaultAsync(x => x.TokenHash == hash, ct);

        if (existing is null)
            return Result.Failure<TokenPair>(IdentityErrors.InvalidRefreshToken);

        if (!existing.IsActive)
        {
            await RevokeAllAsync(new UserId(existing.UserId), ct);
            return Result.Failure<TokenPair>(IdentityErrors.InvalidRefreshToken);
        }

        var newPair = BuildTokenPair(existing.UserId);
        var newToken = RefreshToken.Create(existing.UserId, Hash(newPair.RefreshToken), newPair.RefreshTokenExpiresAtUtc);

        existing.Revoke(newToken.Id);   // rotation
        db.RefreshTokens.Add(newToken);

        await db.SaveChangesAsync(ct);
        return Result.Success(newPair);
    }

    public async Task<Result> RevokeAsync(string refreshToken, CancellationToken ct = default)
    {
        var hash = Hash(refreshToken);
        var existing = await db.RefreshTokens.FirstOrDefaultAsync(x => x.TokenHash == hash, ct);

        if (existing is null || !existing.IsActive)
            return Result.Failure(IdentityErrors.InvalidRefreshToken);

        existing.Revoke();
        await db.SaveChangesAsync(ct);
        return Result.Success();
    }

    public async Task RevokeAllAsync(UserId userId, CancellationToken ct = default)
    {
        var tokens = await db.RefreshTokens
            .Where(x => x.UserId == userId.Value && x.RevokedAtUtc == null)
            .ToListAsync(ct);

        foreach (var token in tokens) token.Revoke();
        await db.SaveChangesAsync(ct);
    }

    private TokenPair BuildTokenPair(Guid userId)
    {
        var (accessToken, accessExpires) = JwtTokenFactory.CreateAccessToken(userId, _settings);

        var refreshExpires = DateTime.UtcNow.AddDays(_settings.RefreshTokenExpiryDays);

        var refreshToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));

        return new TokenPair(userId, accessToken, accessExpires, refreshToken, refreshExpires);
    }

    private static string Hash(string value) =>
        Convert.ToHexString(SHA256.HashData(System.Text.Encoding.UTF8.GetBytes(value)));
}