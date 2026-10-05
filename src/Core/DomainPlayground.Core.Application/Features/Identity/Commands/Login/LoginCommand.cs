using DomainPlayground.Core.Application.Common.Abstractions.Messaging;
using DomainPlayground.Core.Application.Common.Interfaces;
using DomainPlayground.Core.Application.Features.Identity.Interfaces;
using DomainPlayground.Core.Application.Features.Identity.Models;
using DomainPlayground.SharedKernel.Results;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace DomainPlayground.Core.Application.Features.Identity.Commands.Login
{
    public sealed record LoginCommand(string UserName, string Password) : ICommand<AuthResponse>;
    public sealed class LoginCommandValidator : AbstractValidator<LoginCommand>
    {
        public LoginCommandValidator()
        {
            RuleFor(x => x.UserName).NotEmpty();
            RuleFor(x => x.Password).NotEmpty();
        }
    }
    internal sealed class LoginCommandHandler(
    IIdentityService identityService, ITokenService tokenService)
    : ICommandHandler<LoginCommand, AuthResponse>
    {
        public async Task<Result<AuthResponse>> Handle(LoginCommand cmd, CancellationToken ct)
        {
            var validated = await identityService.ValidateCredentialsAsync(cmd.UserName, cmd.Password, ct);
            if (validated.IsFailure) return Result.Failure<AuthResponse>(validated.Error);

            var tokens = await tokenService.IssueAsync(validated.Value.UserId, ct);

            return Result.Success(new AuthResponse(
                validated.Value.UserId.Value, tokens.AccessToken, tokens.AccessTokenExpiresAtUtc,
                tokens.RefreshToken, tokens.RefreshTokenExpiresAtUtc));
        }
    }
}
