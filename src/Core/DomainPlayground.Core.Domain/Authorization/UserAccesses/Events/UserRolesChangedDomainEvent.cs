using DomainPlayground.Core.Domain.Authorization.UserAccesses.ValueObjects;
using DomainPlayground.SharedKernel.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace DomainPlayground.Core.Domain.Authorization.UserAccesses.Events
{
    public sealed record UserRolesChangedDomainEvent(UserId userId) : IDomainEvent
    {
        public DateTime OccurredOnUtc { get; set; }
    }
}
