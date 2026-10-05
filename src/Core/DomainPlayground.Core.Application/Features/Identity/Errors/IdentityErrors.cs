using DomainPlayground.SharedKernel.Errors;
using System;
using System.Collections.Generic;
using System.Text;

namespace DomainPlayground.Core.Application.Features.Identity.Errors;

public static class IdentityErrors
{
    public static readonly Error InvalidCredentials = new("Identity.InvalidCredentials", ErrorType.Unauthorized);
    public static readonly Error LockedOut = new("Identity.LockedOut", ErrorType.Forbidden);
    public static readonly Error UserNameTaken = new("Identity.UserNameTaken", ErrorType.Conflict);
    public static readonly Error WeakPassword = new("Identity.WeakPassword", ErrorType.Validation);
    public static readonly Error InvalidRefreshToken = new("Identity.InvalidRefreshToken", ErrorType.Unauthorized);
}