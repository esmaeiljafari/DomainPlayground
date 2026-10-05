using DomainPlayground.Core.Application.Common.Abstractions.Messaging;
using DomainPlayground.Core.Application.Features.Authorization.Interfaces;
using DomainPlayground.Core.Domain.Authorization.Permissions.Errors;
using DomainPlayground.Core.Domain.Authorization.Permissions.ValueObjects;
using DomainPlayground.Core.Domain.Authorization.Roles.Errors;
using DomainPlayground.Core.Domain.Authorization.Roles.ValueObjects;
using DomainPlayground.SharedKernel.Results;

namespace DomainPlayground.Core.Application.Features.Authorization.Roles.Commands.UpdateRole
{
    internal sealed class UpdateRoleCommandHandler(
    IRoleRepository roles, IPermissionRepository permissions)
    : ICommandHandler<UpdateRoleCommand>
    {
        public async Task<Result> Handle(UpdateRoleCommand cmd, CancellationToken ct)
        {
            var role = await roles.GetByIdAsync(new RoleId(cmd.RoleId), ct);
            if (role is null) return Result.Failure(RoleErrors.NotFound);

            var requestedIds = cmd.PermissionIds.Select(id => new PermissionId(id)).ToList();
            var existing = await permissions.GetByIdsAsync(requestedIds, ct);
            if (existing.Count != requestedIds.Distinct().Count())
                return Result.Failure(PermissionErrors.NotFound);

            var rename = role.UpdateDetails(cmd.Name);
            if (rename.IsFailure) return Result.Failure(rename.Error);

            var setPermissions = role.SetPermissions(requestedIds);
            if (setPermissions.IsFailure) return Result.Failure(setPermissions.Error);

            return Result.Success();
        }
    }
}
