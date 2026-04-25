using WebApp.Domain.Common.Results;
using WebApp.Domain.Common.ValueObjects;
using WebApp.Domain.EmployeeAggregate.Errors;

namespace WebApp.Domain.EmployeeAggregate.ValueObjects;

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
