using System;
using System.Collections.Generic;
using System.Text;

namespace DomainPlayground.Core.Application.Features.Authorization.UserAccesses.Queries.GetUserRoles
{
    public sealed record UserRoleDto(int RoleId, string Name, bool IsAssigned);
}
