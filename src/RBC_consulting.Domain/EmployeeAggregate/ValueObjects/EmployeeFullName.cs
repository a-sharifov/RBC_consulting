using RBC_consulting.Domain.Common.Results;
using RBC_consulting.Domain.Common.ValueObjects;
using RBC_consulting.Domain.EmployeeAggregate.Errors;

namespace RBC_consulting.Domain.EmployeeAggregate.ValueObjects;

public sealed class EmployeeFullName : ValueObject
{
    public const int MaxLength = 200;
    public string Value { get; }

    private EmployeeFullName(string value) => Value = value;

    public static Result<EmployeeFullName> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return Result.Failure<EmployeeFullName>(EmployeeFullNameErrors.Empty);

        if (value.Length > MaxLength)
            return Result.Failure<EmployeeFullName>(EmployeeFullNameErrors.TooLong);

        return Result.Success(new EmployeeFullName(value.Trim()));
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public static implicit operator string(EmployeeFullName fullName) => fullName.Value;
}
