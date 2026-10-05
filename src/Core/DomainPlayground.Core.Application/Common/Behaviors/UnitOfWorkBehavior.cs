using DomainPlayground.Core.Application.Common.Abstractions.Messaging;
using DomainPlayground.Core.Application.Common.Interfaces.Persistence;
using DomainPlayground.SharedKernel.Results;
using MediatR;

namespace DomainPlayground.Core.Application.Common.Behaviors;

public sealed class UnitOfWorkBehavior<TRequest, TResponse>(IUnitOfWork uow)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : ICommandMarker
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken ct)
    {
        Console.WriteLine($"[UOW-ENTER] {typeof(TRequest).Name}");

        var response = await next();

        if (IsSuccessfulResponse(response))
        {
            var affected = await uow.SaveChangesAsync(ct);
            Console.WriteLine($"[UOW-SAVED] {affected} rows affected");
        }

        return response;
    }

    private static bool IsSuccessfulResponse(TResponse? response)
    {
        if (response is null) return false;

        if (response is Result result)
        {
            return result.IsSuccess;
        }

        var isSuccessProperty = response.GetType().GetProperty(nameof(Result.IsSuccess));
        return isSuccessProperty?.GetValue(response) is true;
    }
}