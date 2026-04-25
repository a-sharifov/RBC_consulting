using MediatR;

namespace WebApp.Domain.Common.Events.Interfaces;

public interface IDomainEventHandler<TDomainEvent> : INotificationHandler<TDomainEvent>
    where TDomainEvent : IDomainEvent;
