using MediatR;

namespace RBC_consulting.Domain.Common.Events.Interfaces;

public interface IDomainEventHandler<TDomainEvent> : INotificationHandler<TDomainEvent>
    where TDomainEvent : IDomainEvent;
