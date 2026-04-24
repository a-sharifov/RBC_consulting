namespace RBC_consulting.Domain.EmployeeAggregate.Ids;

public readonly struct EmployeeId(int value) : IEquatable<EmployeeId>
{
    public int Value { get; } = value;

    public bool Equals(EmployeeId other) =>
        Value == other.Value;

    public static implicit operator EmployeeId(int value) => new(value);
    public static implicit operator int(EmployeeId id) => id.Value;
}
