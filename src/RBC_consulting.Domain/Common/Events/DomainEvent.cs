using RBC_consulting.Domain.Common.Events.Interfaces;

namespace RBC_consulting.Domain.Common.Events;

public abstract record DomainEvent(Guid Id) : IDomainEvent;
