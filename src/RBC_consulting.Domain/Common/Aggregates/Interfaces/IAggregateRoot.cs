using RBC_consulting.Domain.Common.Entities.Interfaces;

namespace RBC_consulting.Domain.Common.Aggregates.Interfaces;

public interface IAggregateRoot<TId> : IEntity<TId>;
