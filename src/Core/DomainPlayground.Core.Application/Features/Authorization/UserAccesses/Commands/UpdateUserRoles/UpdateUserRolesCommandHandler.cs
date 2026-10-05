using DomainPlayground.Core.Application.Common.Abstractions.Messaging;
using DomainPlayground.Core.Application.Features.Authorization.Interfaces;
using DomainPlayground.Core.Domain.Authorization.Roles.Errors;
using DomainPlayground.Core.Domain.Authorization.Roles.ValueObjects;
using DomainPlayground.Core.Domain.Authorization.UserAccesses.Errors;
using DomainPlayground.Core.Domain.Authorization.UserAccesses.ValueObjects;
using DomainPlayground.SharedKernel.Results;

namespace DomainPlayground.Core.Application.Features.Authorization.UserAccesses.Commands.UpdateUserRoles
{
    internal sealed class UpdateUserRolesCommandHandler(
    IUserAccessRepository userAccesses, IRoleRepository roles)
    : ICommandHandler<UpdateUserRolesCommand>
    {
        public async Task<Result> Handle(UpdateUserRolesCommand cmd, CancellationToken ct)
        {
            var access = await userAccesses.GetByUserIdAsync(new UserId(cmd.UserId), ct);
            if (access is null) return Result.Failure(UserAccessErrors.NotFound);

            var requestedIds = cmd.RoleIds.Select(id => new RoleId(id)).ToList();
            var existing = await roles.GetByIdsAsync(requestedIds, ct);
            if (existing.Count != requestedIds.Distinct().Count())
                return Result.Failure(RoleErrors.NotFound);

            access.SetRoles(requestedIds);
            return Result.Success();
        }
    }
}
