using RBC_consulting.Domain.Common.Aggregates.Interfaces;
using RBC_consulting.Domain.Common.Entities;

namespace RBC_consulting.Domain.Common.Aggregates;

public abstract class AggregateRoot<TStrongestId>
    : Entity<TStrongestId>, IAggregateRoot<TStrongestId>
    where TStrongestId : struct, IEquatable<TStrongestId>
{
    protected AggregateRoot() { }

    protected AggregateRoot(TStrongestId id) : base(id) { }
}
