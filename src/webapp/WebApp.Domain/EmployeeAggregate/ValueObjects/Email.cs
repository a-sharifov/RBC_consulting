using System.Text.RegularExpressions;
using WebApp.Domain.Common.Results;
using WebApp.Domain.Common.ValueObjects;
using WebApp.Domain.EmployeeAggregate.Errors;

namespace WebApp.Domain.EmployeeAggregate.ValueObjects;

public sealed partial class Email : ValueObject
{
    public const int MaxLength = 255;


    private static readonly Regex EmailRegex = EmailRegexExpression();
    public string Value { get; }

    private Email(string value) => Value = value;

    public static Result<Email> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return Result.Failure<Email>(EmailErrors.Empty);

        if (!EmailRegex.IsMatch(value))
            return Result.Failure<Email>(EmailErrors.Invalid);

        if (value.Length > MaxLength)
            return Result.Failure<Email>(EmailErrors.TooLong);

        return Result.Success<Email>(new Email(value.Trim()));
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public static implicit operator string(Email email) => email?.Value ?? string.Empty;


    [GeneratedRegex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.IgnoreCase | RegexOptions.Compiled, "ru-RU")]
    private static partial Regex EmailRegexExpression();
}
