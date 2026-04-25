using WebApp.Domain.Common.Results;
using WebApp.Domain.Common.ValueObjects;
using WebApp.Domain.EmployeeAggregate.Errors;

namespace WebApp.Domain.EmployeeAggregate.ValueObjects;

public sealed class Position : ValueObject
{
    public const int MaxLength = 100;
    public string Value { get; }

    private Position(string value) => Value = value;

    public static Result<Position> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return Result.Failure<Position>(PositionErrors.Empty);

        if (value.Length > MaxLength)
            return Result.Failure<Position>(PositionErrors.TooLong);

        return Result.Success(new Position(value.Trim()));
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public static implicit operator string(Position position) => position?.Value ?? string.Empty;
}
