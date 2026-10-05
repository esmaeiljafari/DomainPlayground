using DomainPlayground.Core.Application.Common.Abstractions.Messaging;
using DomainPlayground.Core.Application.Common.Abstractions.Security;
using DomainPlayground.SharedKernel.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace DomainPlayground.Core.Application.Features.Authorization.Roles.Commands.DisableRole;

[HasPermission(ModuleCodes.Authorization, "roles.disable", "غیرفعال‌سازی نقش")]
public sealed record DisableRoleCommand(int RoleId) : ICommand;
