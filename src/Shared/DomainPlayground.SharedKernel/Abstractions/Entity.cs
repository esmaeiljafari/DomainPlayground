namespace DomainPlayground.SharedKernel.Abstractions;

public abstract class Entity<TId> : IEntity where TId : notnull
{
    public TId Id { get; protected set; } = default!;
    public DateTime CreatedAt { get; protected set; }
    public DateTime? UpdatedAt { get; protected set; }

    protected Entity() { }
    protected Entity(TId id) => Id = id;

    public override bool Equals(object? obj)
    {
        if (obj is not Entity<TId> other) return false;
        if (ReferenceEquals(this, other)) return true;
        if (GetType() != other.GetType()) return false;
        if (IsTransient() || other.IsTransient()) return false;
        return EqualityComparer<TId>.Default.Equals(Id, other.Id);
    }

    public override int GetHashCode() => HashCode.Combine(GetType(), Id);
    protected bool IsTransient() => EqualityComparer<TId>.Default.Equals(Id, default!);
    protected void MarkUpdated() => UpdatedAt = DateTime.UtcNow;

    public static bool operator ==(Entity<TId>? l, Entity<TId>? r) =>
        l is null ? r is null : l.Equals(r);
    public static bool operator !=(Entity<TId>? l, Entity<TId>? r) => !(l == r);
}