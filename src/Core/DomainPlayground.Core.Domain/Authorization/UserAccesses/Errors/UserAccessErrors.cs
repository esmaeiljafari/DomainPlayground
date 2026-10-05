using DomainPlayground.SharedKernel.Errors;
using System;
using System.Collections.Generic;
using System.Text;

namespace DomainPlayground.Core.Domain.Authorization.UserAccesses.Errors;

public static class UserAccessErrors
{
    public static readonly Error RoleAlreadyAssigned = new("UserAccess.RoleAlreadyAssigned", ErrorType.Conflict);
    public static readonly Error RoleNotAssigned = new("UserAccess.RoleNotAssigned", ErrorType.NotFound);
    public static readonly Error NotFound = new("UserAccess.NotFound", ErrorType.NotFound);
}