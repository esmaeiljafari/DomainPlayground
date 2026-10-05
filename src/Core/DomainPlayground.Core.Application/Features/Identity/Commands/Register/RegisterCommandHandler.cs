using DomainPlayground.Core.Application.Common.Abstractions.Messaging;
using DomainPlayground.Core.Application.Common.Interfaces;
using DomainPlayground.Core.Application.Features.Authorization.Interfaces;
using DomainPlayground.Core.Application.Features.Identity.Interfaces;
using DomainPlayground.Core.Application.Features.Identity.Models;
using DomainPlayground.Core.Domain.Authorization.UserAccesses;
using DomainPlayground.Core.Domain.Authorization.UserAccesses.ValueObjects;
using DomainPlayground.SharedKernel.Results;

namespace DomainPlayground.Core.Application.Features.Identity.Commands.Register
{
    internal sealed class RegisterCommandHandler(
    IIdentityService identityService,
    ITokenService tokenService,
    IUserAccessRepository userAccessRepository)
    : ICommandHandler<RegisterCommand, AuthResponse>
    {
        public async Task<Result<AuthResponse>> Handle(RegisterCommand cmd, CancellationToken ct)
        {
            var userId = new UserId(Guid.NewGuid());

            var created = await identityService.CreateUserAsync(userId, cmd.UserName, cmd.Password, ct);
            if (created.IsFailure)
                return Result.Failure<AuthResponse>(created.Error);

            var access = UserAccess.Create(userId);
            await userAccessRepository.AddAsync(access, ct);

            var tokens = await tokenService.IssueAsync(userId, ct);

            return Result.Success(new AuthResponse(
                userId.Value, tokens.AccessToken, tokens.AccessTokenExpiresAtUtc,
                tokens.RefreshToken, tokens.RefreshTokenExpiresAtUtc));
        }
    }
}
