using DomainPlayground.Core.Application.Common.Abstractions.Messaging;
using DomainPlayground.Core.Application.Features.Authorization.Interfaces;
using DomainPlayground.Core.Domain.Authorization.Roles.Errors;
using DomainPlayground.Core.Domain.Authorization.Roles.ValueObjects;
using DomainPlayground.SharedKernel.Results;

namespace DomainPlayground.Core.Application.Features.Authorization.Roles.Commands.DisableRole;

internal sealed class DisableRoleCommandHandler(IRoleRepository roles) : ICommandHandler<DisableRoleCommand>
{
    public async Task<Result> Handle(DisableRoleCommand cmd, CancellationToken ct)
    {
        var role = await roles.GetByIdAsync(new RoleId(cmd.RoleId), ct);
        if (role is null) return Result.Failure(RoleErrors.NotFound);

        return role.Disable();
    }
}
