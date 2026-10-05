namespace DomainPlayground.SharedKernel.Abstractions;

public interface IDomainEvent
{
    DateTime OccurredOnUtc { get; }
}
