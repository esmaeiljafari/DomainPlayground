using System.Collections.Concurrent;
using System.Reflection;
using DomainPlayground.SharedKernel.Errors;
using DomainPlayground.SharedKernel.Results;
using FluentValidation;
using MediatR;

namespace DomainPlayground.Core.Application.Common.Behaviors
{
    public sealed class ValidationBehavior<TRequest, TResponse>(
        IEnumerable<IValidator<TRequest>> validators)
        : IPipelineBehavior<TRequest, TResponse>
        where TRequest : notnull
        where TResponse : Result
    {
        private static readonly ConcurrentDictionary<Type, MethodInfo> FailureMethodCache = new();

        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken ct)
        {
            Console.WriteLine($"[VALIDATION-ENTER] {typeof(TRequest).Name}");

            if (!validators.Any())
                return await next();

            var context = new ValidationContext<TRequest>(request);

            var results = await Task.WhenAll(
                validators.Select(v => v.ValidateAsync(context, ct)));

            var failures = results
                .SelectMany(r => r.Errors)
                .Where(f => f is not null)
                .ToList();

            if (failures.Count == 0)
                return await next();

            var error = new Error("Validation.Failed");

            return CreateFailureResult(error);
        }

        private static TResponse CreateFailureResult(Error error)
        {
            if (typeof(TResponse) == typeof(Result))
                return (TResponse)Result.Failure(error);

            var valueType = typeof(TResponse).GetGenericArguments()[0];

            var failureMethod = FailureMethodCache.GetOrAdd(valueType, type =>
                typeof(Result)
                    .GetMethods()
                    .First(m => m.Name == nameof(Result.Failure) && m.IsGenericMethodDefinition)
                    .MakeGenericMethod(type));

            return (TResponse)failureMethod.Invoke(null, [error])!;
        }
    }
}