using WebApp.Domain.Common.Entities.Interfaces;

namespace WebApp.Domain.Common.Aggregates.Interfaces;

public interface IAggregateRoot<TId> : IEntity<TId>;
