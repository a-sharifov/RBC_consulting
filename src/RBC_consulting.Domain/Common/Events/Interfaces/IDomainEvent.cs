using MediatR;

namespace RBC_consulting.Domain.Common.Events.Interfaces;

public interface IDomainEvent : INotification
{
    Guid Id { get; }
}
