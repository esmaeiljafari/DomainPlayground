using DomainPlayground.SharedKernel.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace DomainPlayground.Core.Domain.Authorization.Permissions.Events
{
    public sealed record PermissionRegisteredDomainEvent(string Key) : IDomainEvent
    {
        public DateTime OccurredOnUtc { get; } = DateTime.UtcNow;
    }

    public sealed record PermissionDeprecatedDomainEvent(string Key) : IDomainEvent
    {
        public DateTime OccurredOnUtc { get; } = DateTime.UtcNow;
    }
}
