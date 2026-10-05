using System.Reflection;
using DomainPlayground.Core.Application.Common.Abstractions.Security;
using DomainPlayground.Core.Application.Common.Interfaces;
using DomainPlayground.Core.Application.Features.Authorization.Interfaces;
using DomainPlayground.SharedKernel.Errors;
using DomainPlayground.SharedKernel.Results;
using MediatR;

namespace DomainPlayground.Core.Application.Common.Behaviors;

public sealed class AuthorizationBehavior<TRequest, TResponse>(
    ICurrentUser currentUser, IAuthorizationQueries authQueries)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
    where TResponse : Result
{
    public async Task<TResponse> Handle(
        TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken ct)
    {
        var attribute = request.GetType().GetCustomAttribute<HasPermissionAttribute>();
        if (attribute is null)
            return await next();

        if (!currentUser.IsAuthenticated || currentUser.UserId is null)
            return CreateFailure(new Error("Auth.Unauthenticated", ErrorType.Unauthorized));

        var requiredKey = $"{attribute.ModuleCode.ToLowerInvariant()}.{attribute.PermissionCode.ToLowerInvariant()}";

        var userPermissions = await authQueries.GetUserPermissionKeysAsync(currentUser.UserId.Value.Value, ct);

        if (!userPermissions.Contains(requiredKey))
            return CreateFailure(new Error("Auth.Forbidden", ErrorType.Forbidden));

        return await next();
    }

    private static TResponse CreateFailure(Error error)
    {
        if (typeof(TResponse) == typeof(Result))
            return (TResponse)Result.Failure(error);

        var valueType = typeof(TResponse).GetGenericArguments()[0];
        var method = typeof(Result)
            .GetMethods()
            .First(m => m.Name == nameof(Result.Failure) && m.IsGenericMethodDefinition)
            .MakeGenericMethod(valueType);

        return (TResponse)method.Invoke(null, [error])!;
    }
}