using DomainPlayground.Core.Application.Common.Abstractions.Messaging;
using DomainPlayground.Core.Application.Common.Abstractions.Security;
using DomainPlayground.SharedKernel.Abstractions;

namespace DomainPlayground.Core.Application.Features.Authorization.UserAccesses.Queries.GetUserRoles
{
    [HasPermission(ModuleCodes.Authorization, "userAccesses.view", "مشاهده دسترسی کاربر")]
    public sealed record GetUserRolesQuery(Guid UserId) : IQuery<List<UserRoleDto>>;
}
