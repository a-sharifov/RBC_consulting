namespace WebApp.Domain.Common.Entities.Interfaces;

public interface IEntity<TId>
{
    TId Id { get; }
}
