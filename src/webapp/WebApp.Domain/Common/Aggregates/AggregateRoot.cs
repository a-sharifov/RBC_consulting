using WebApp.Domain.Common.Aggregates.Interfaces;
using WebApp.Domain.Common.Entities;

namespace WebApp.Domain.Common.Aggregates;

public abstract class AggregateRoot<TStrongestId>
    : Entity<TStrongestId>, IAggregateRoot<TStrongestId>
    where TStrongestId : struct, IEquatable<TStrongestId>
{
    protected AggregateRoot() { }

    protected AggregateRoot(TStrongestId id) : base(id) { }
}
