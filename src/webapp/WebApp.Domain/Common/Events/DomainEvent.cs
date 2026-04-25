using WebApp.Domain.Common.Events.Interfaces;

namespace WebApp.Domain.Common.Events;

public abstract record DomainEvent(Guid Id) : IDomainEvent;
