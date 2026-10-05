using DomainPlayground.Core.Application.Common.Abstractions.Messaging;
using DomainPlayground.Core.Application.Features.Identity.Models;

namespace DomainPlayground.Core.Application.Features.Identity.Commands.Register
{
    public sealed record RegisterCommand(string UserName, string Password) : ICommand<AuthResponse>;
}
