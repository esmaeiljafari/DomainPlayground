
using DomainPlayground.SharedKernel.Errors;

namespace DomainPlayground.Core.Domain.Authorization.Permissions.Errors;
public static class PermissionErrors
{
    public static readonly Error InvalidKey = new("Permission.InvalidKey", ErrorType.Validation);
    public static readonly Error DescriptionRequired = new("Permission.DescriptionRequired", ErrorType.Validation);
    public static readonly Error NotFound = new("Permission.NotFound", ErrorType.NotFound);
}