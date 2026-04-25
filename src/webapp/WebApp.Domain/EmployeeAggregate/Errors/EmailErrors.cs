using WebApp.Domain.Common.Errors;
using WebApp.Domain.EmployeeAggregate.ValueObjects;

namespace WebApp.Domain.EmployeeAggregate.Errors;

public static class EmailErrors
{
    public static readonly Error Invalid = Error.Validation(
        "Email.Invalid",
        "Email format is invalid");

    public static readonly Error TooLong = Error.Validation(
        "Email.TooLong",
        $"Email cannot exceed {Email.MaxLength} characters");

    public static readonly Error Empty = Error.Validation(
        "Email.Empty",
        "Email cannot be empty");
}
