using RBC_consulting.Domain.Common.ValueObjects.Interfaces;

namespace RBC_consulting.Domain.Common.ValueObjects;

public abstract class ValueObject : IValueObject
{
    public abstract IEnumerable<object> GetEqualityComponents();

    public bool Equals(IValueObject? other) =>
        other is not null && ValuesAreEqual(other);

    public override bool Equals(object? obj) =>
        obj is IValueObject other && ValuesAreEqual(other);

    public override int GetHashCode() =>
        GetEqualityComponents().Aggregate(default(int), HashCode.Combine);

    private bool ValuesAreEqual(IValueObject other) =>
        GetEqualityComponents().SequenceEqual(other.GetEqualityComponents());

    public static bool operator ==(ValueObject? left, ValueObject? right) =>
        Equals(left, right);

    public static bool operator !=(ValueObject? left, ValueObject? right) =>
        !Equals(left, right);
}
