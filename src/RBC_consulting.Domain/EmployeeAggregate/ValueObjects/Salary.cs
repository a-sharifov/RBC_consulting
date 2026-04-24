using RBC_consulting.Domain.Common.Results;
using RBC_consulting.Domain.Common.ValueObjects;
using RBC_consulting.Domain.EmployeeAggregate.Errors;

namespace RBC_consulting.Domain.EmployeeAggregate.ValueObjects;

public sealed class Salary : ValueObject
{
    public const decimal MaxValue = 9_999_999.99m;
    public decimal Value { get; }

    private Salary(decimal value) => Value = value;

    public static Result<Salary> Create(decimal value)
    {
        if (value < 0)
            return Result.Failure<Salary>(SalaryErrors.Negative);

        if (value > MaxValue)
            return Result.Failure<Salary>(SalaryErrors.TooHigh);

        return Result.Success(new Salary(value));
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public static implicit operator decimal(Salary salary) => salary?.Value ?? 0m;
    public static implicit operator decimal?(Salary? salary) => salary?.Value;
}
