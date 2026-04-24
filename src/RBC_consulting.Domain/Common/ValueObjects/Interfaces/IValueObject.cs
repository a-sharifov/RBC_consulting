namespace RBC_consulting.Domain.Common.ValueObjects.Interfaces;

public interface IValueObject : IEquatable<IValueObject>
{
    IEnumerable<object> GetEqualityComponents();
}
