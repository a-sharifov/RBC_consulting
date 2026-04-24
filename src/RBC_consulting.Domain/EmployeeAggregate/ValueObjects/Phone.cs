using System.Text.RegularExpressions;
using RBC_consulting.Domain.Common.Results;
using RBC_consulting.Domain.Common.ValueObjects;
using RBC_consulting.Domain.EmployeeAggregate.Errors;

namespace RBC_consulting.Domain.EmployeeAggregate.ValueObjects;

public sealed partial class Phone : ValueObject
{
    public const int MaxLength = 20;

    private static readonly Regex PhoneRegex = PhoneRegexExpression();
    public string Value { get; }

    private Phone(string value) => Value = value;

    public static Result<Phone> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return Result.Failure<Phone>(PhoneErrors.Empty);

        if (value.Length > MaxLength)
            return Result.Failure<Phone>(PhoneErrors.TooLong);

        if (!PhoneRegex.IsMatch(value.Replace("-", "").Replace(" ", "")))
            return Result.Failure<Phone>(PhoneErrors.Invalid);

        return Result.Success(new Phone(value.Trim()));
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public static implicit operator string(Phone phone) => phone?.Value ?? string.Empty;

    [GeneratedRegex(@"^\+?[1-9]\d{1,14}$", RegexOptions.Compiled)]
    private static partial Regex PhoneRegexExpression();
}
