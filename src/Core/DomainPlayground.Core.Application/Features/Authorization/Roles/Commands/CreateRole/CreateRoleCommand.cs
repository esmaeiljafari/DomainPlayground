using DomainPlayground.Core.Application.Common.Abstractions.Messaging;
using DomainPlayground.Core.Application.Common.Abstractions.Security;
using DomainPlayground.SharedKernel.Abstractions;

namespace DomainPlayground.Core.Application.Features.Authorization.Roles.Commands.CreateRole;

[HasPermission(ModuleCodes.Authorization, "roles.create", "ایجاد نقش")]
public sealed record CreateRoleCommand(string Name) : ICommand;
