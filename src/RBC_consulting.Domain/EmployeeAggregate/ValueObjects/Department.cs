using RBC_consulting.Domain.Common.Results;
using RBC_consulting.Domain.Common.ValueObjects;
using RBC_consulting.Domain.EmployeeAggregate.Errors;

namespace RBC_consulting.Domain.EmployeeAggregate.ValueObjects;

public sealed class Department : ValueObject
{
    public const int MaxLength = 100;

    public string Value { get; }

    private Department(string value) => Value = value;

    public static Result<Department> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return Result.Failure<Department>(DepartmentErrors.Empty);

        if (value.Length > MaxLength)
            return Result.Failure<Department>(DepartmentErrors.TooLong);

        return Result.Success(new Department(value.Trim()));
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public static implicit operator string(Department department) => department?.Value ?? string.Empty;
}
