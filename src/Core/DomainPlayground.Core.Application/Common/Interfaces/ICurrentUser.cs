using DomainPlayground.Core.Domain.Authorization.UserAccesses.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace DomainPlayground.Core.Application.Common.Interfaces
{
    public interface ICurrentUser
    {
        UserId? UserId { get; }
        bool IsAuthenticated { get; }
    }
}
