using DomainPlayground.Core.Application.Common.Abstractions.Messaging;
using DomainPlayground.Core.Application.Common.Abstractions.Security;
using DomainPlayground.SharedKernel.Abstractions;
using DomainPlayground.SharedKernel.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace DomainPlayground.Core.Application.Features.Authorization.Roles.Queries.GetAllRoles
{
    [HasPermission(ModuleCodes.Authorization, "userAccesses.roleView", "دریافت لیست نقش‌ها")]
    public sealed record GetAllRolesQuery : IQuery<List<RoleDto>>;
}
