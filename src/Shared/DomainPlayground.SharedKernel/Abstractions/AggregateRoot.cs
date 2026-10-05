using System.Collections.Generic;

namespace DomainPlayground.SharedKernel.Abstractions;

/// <summary>
/// Base class for Aggregate Roots. Owns and raises domain events that get
/// dispatched (e.g. by an interceptor/UnitOfWork) after persistence.
/// </summary>
public abstract class AggregateRoot<TId> : Entity<TId>, IAggregateRoot where TId : notnull
{
    private readonly List<IDomainEvent> _domainEvents = [];

    protected AggregateRoot()
    {
    }

    protected AggregateRoot(TId id) : base(id)
    {
    }

    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    protected void AddDomainEvent(IDomainEvent domainEvent) => _domainEvents.Add(domainEvent);

    public void ClearDomainEvents() => _domainEvents.Clear();
}
