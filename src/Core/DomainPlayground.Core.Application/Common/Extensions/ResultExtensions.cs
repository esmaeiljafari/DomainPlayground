using DomainPlayground.SharedKernel.Errors;
using DomainPlayground.SharedKernel.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace DomainPlayground.Core.Application.Common.Extensions
{
    public static class ResultExtensions
    {
        public static TOut Match<TOut>(this Result result, Func<TOut> onSuccess, Func<Error, TOut> onFailure) =>
            result.IsSuccess ? onSuccess() : onFailure(result.Error);

        public static TOut Match<T, TOut>(this Result<T> result, Func<T, TOut> onSuccess, Func<Error, TOut> onFailure) =>
            result.IsSuccess ? onSuccess(result.Value) : onFailure(result.Error);
    }


}
