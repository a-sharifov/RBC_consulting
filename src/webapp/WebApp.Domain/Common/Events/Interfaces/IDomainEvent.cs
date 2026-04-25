using MediatR;

namespace WebApp.Domain.Common.Events.Interfaces;

public interface IDomainEvent : INotification
{
    Guid Id { get; }
}
