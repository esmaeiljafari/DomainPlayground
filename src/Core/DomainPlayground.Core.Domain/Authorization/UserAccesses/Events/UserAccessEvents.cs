using DomainPlayground.Core.Domain.Authorization.Roles.ValueObjects;
using DomainPlayground.Core.Domain.Authorization.UserAccesses.ValueObjects;
using DomainPlayground.SharedKernel.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace DomainPlayground.Core.Domain.Authorization.UserAccesses.Events
{
    // UserAccesses/Events/UserAccessEvents.cs
    public sealed record UserRoleAssignedDomainEvent(UserId UserId, RoleId RoleId) : IDomainEvent
    {
        public DateTime OccurredOnUtc { get; } = DateTime.UtcNow;
    }

    public sealed record UserRoleRemovedDomainEvent(UserId UserId, RoleId RoleId) : IDomainEvent
    {
        public DateTime OccurredOnUtc { get; } = DateTime.UtcNow;
    }

    public sealed record UserOverridesChangedDomainEvent(UserId UserId) : IDomainEvent
    {
        public DateTime OccurredOnUtc { get; } = DateTime.UtcNow;
    }
}
