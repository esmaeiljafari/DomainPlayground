using DomainPlayground.Core.Application.Common.Abstractions.Messaging;
using DomainPlayground.Core.Application.Features.Identity.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DomainPlayground.Core.Application.Features.Identity.Commands.RefreshToken
{
    public sealed record RefreshTokenCommand(string RefreshToken) : ICommand<AuthResponse>;
}
