using DomainPlayground.Core.Application.Common.Abstractions.Messaging;
using DomainPlayground.Core.Application.Features.Identity.Interfaces;
using DomainPlayground.SharedKernel.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace DomainPlayground.Core.Application.Features.Identity.Commands.Logout
{
    public sealed record LogoutCommand(string RefreshToken) : ICommand;

    internal sealed class LogoutCommandHandler(ITokenService tokenService) : ICommandHandler<LogoutCommand>
    {
        public async Task<Result> Handle(LogoutCommand cmd, CancellationToken ct) =>
            await tokenService.RevokeAsync(cmd.RefreshToken, ct);
    }
}
