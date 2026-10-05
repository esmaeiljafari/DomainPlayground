using DomainPlayground.Core.Application.Common.Abstractions.Messaging;
using DomainPlayground.Core.Application.Common.Abstractions.Security;
using DomainPlayground.SharedKernel.Abstractions;

namespace DomainPlayground.Core.Application.Features.Authorization.UserAccesses.Commands.UpdateUserRoles
{
    [HasPermission(ModuleCodes.Authorization, "userAccesses.update", "ویرایش دسترسی کاربر")]
    public sealed record UpdateUserRolesCommand(Guid UserId, List<int> RoleIds) : ICommand;
}
