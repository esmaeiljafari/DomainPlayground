using DomainPlayground.Core.Domain.Authorization.Roles.ValueObjects;
using DomainPlayground.SharedKernel.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace DomainPlayground.Core.Domain.Authorization.Roles.Events
{
    public sealed record RolePermissionsChangedDomainEvent(RoleId RoleId) : IDomainEvent
    {
        public DateTime OccurredOnUtc { get; } = DateTime.UtcNow;
    }
    public sealed record RoleDisabledDomainEvent(RoleId RoleId) : IDomainEvent
    {
        public DateTime OccurredOnUtc { get; } = DateTime.UtcNow;
    }
}
