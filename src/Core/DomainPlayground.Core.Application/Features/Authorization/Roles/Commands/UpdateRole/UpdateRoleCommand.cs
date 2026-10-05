using DomainPlayground.Core.Application.Common.Abstractions.Messaging;
using DomainPlayground.Core.Application.Common.Abstractions.Security;
using DomainPlayground.SharedKernel.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace DomainPlayground.Core.Application.Features.Authorization.Roles.Commands.UpdateRole
{
    [HasPermission(ModuleCodes.Authorization, "roles.update", "ویرایش نقش")]
    public sealed record UpdateRoleCommand(int RoleId, string Name, List<int> PermissionIds) : ICommand;
}
