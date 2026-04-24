using RBC_consulting.Domain.Common.Entities.Interfaces;
using RBC_consulting.Domain.Common.Events.Interfaces;

namespace RBC_consulting.Domain.Common.Entities;

public abstract class Entity<TStrongestId>
    : IEntity<TStrongestId>, IHasDomainEvents
    where TStrongestId : struct, IEquatable<TStrongestId>
{
    private readonly List<IDomainEvent> _domainEvents = [];

    public virtual TStrongestId Id { get; protected set; }

    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    protected Entity() { }

    protected Entity(TStrongestId id) => Id = id;

    protected void AddDomainEvent(IDomainEvent domainEvent) =>
        _domainEvents.Add(domainEvent);

    public void ClearDomainEvents() => _domainEvents.Clear();

    public override bool Equals(object? obj) =>
        obj is Entity<TStrongestId> entity && entity.Id.Equals(Id);

    public override int GetHashCode() => Id.GetHashCode();

    public static bool operator ==(Entity<TStrongestId>? left, Entity<TStrongestId>? right)
    {
        if (ReferenceEquals(left, right)) return true;
        if (left is null || right is null) return false;
        return left.Equals(right);
    }

    public static bool operator !=(Entity<TStrongestId>? left, Entity<TStrongestId>? right) =>
        !(left == right);
}
