using DomainPlayground.SharedKernel.Errors;
using System;
using System.Collections.Generic;
using System.Text;

namespace DomainPlayground.Core.Application.Common.Interfaces;

public interface IErrorLocalizer
{
    string Localize(IError error);
}