namespace RBC_consulting.Domain.Common.Entities.Interfaces;

public interface IEntity<TId>
{
    TId Id { get; }
}
