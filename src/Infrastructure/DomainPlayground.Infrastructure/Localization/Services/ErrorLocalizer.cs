using DomainPlayground.Core.Application.Common.Interfaces;
using DomainPlayground.Infrastructure.Localization.Resources;
using DomainPlayground.SharedKernel.Errors;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Text;

namespace DomainPlayground.Infrastructure.Localization.Services;

internal sealed class ErrorLocalizer(IStringLocalizer<Errors> localizer) : IErrorLocalizer
{
    public string Localize(IError error)
    {
        var result = localizer[error.Code];
        return result.ResourceNotFound ? error.Code : result.Value;
    }
}