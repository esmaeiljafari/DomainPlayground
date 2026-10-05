using DomainPlayground.SharedKernel.Errors;
using System;
using System.Collections.Generic;
using System.Text;

namespace DomainPlayground.Core.Domain.Authorization.Roles.Errors;
public static class RoleErrors
{
    public static readonly Error NameRequired = new("Role.NameRequired", ErrorType.Validation);
    public static readonly Error SystemRoleImmutable = new("Role.SystemImmutable", ErrorType.Forbidden);
    public static readonly Error NameAlreadyExists = new("Role.NameAlreadyExists", ErrorType.Conflict);
    public static readonly Error NotFound = new("Role.NotFound", ErrorType.NotFound);
}
