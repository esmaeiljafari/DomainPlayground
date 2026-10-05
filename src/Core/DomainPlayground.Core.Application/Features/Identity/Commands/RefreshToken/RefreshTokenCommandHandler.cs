using DomainPlayground.Core.Application.Common.Abstractions.Messaging;
using DomainPlayground.Core.Application.Features.Identity.Interfaces;
using DomainPlayground.Core.Application.Features.Identity.Models;
using DomainPlayground.SharedKernel.Results;

namespace DomainPlayground.Core.Application.Features.Identity.Commands.RefreshToken
{
    internal sealed class RefreshTokenCommandHandler(ITokenService tokenService)
        : ICommandHandler<RefreshTokenCommand, AuthResponse>
    {
        public async Task<Result<AuthResponse>> Handle(RefreshTokenCommand cmd, CancellationToken ct)
        {
            var result = await tokenService.RefreshAsync(cmd.RefreshToken, ct);
            if (result.IsFailure) return Result.Failure<AuthResponse>(result.Error);

            var t = result.Value;
            // UserId این‌جا لازم است اگر خروجی باید آن را هم برگرداند؛
            // اگر TokenPair شامل UserId نیست، آن را به مدل اضافه کن.
            return Result.Success(new AuthResponse(
                Guid.Empty, t.AccessToken, t.AccessTokenExpiresAtUtc, t.RefreshToken, t.RefreshTokenExpiresAtUtc));
        }
    }
}
