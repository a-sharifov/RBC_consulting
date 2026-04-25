using WebApp.Domain.Common.Errors;
using WebApp.Domain.EmployeeAggregate.ValueObjects;

namespace WebApp.Domain.EmployeeAggregate.Errors;

public static class EmployeeFullNameErrors
{
    public static readonly Error Empty = Error.Validation(
        "EmployeeFullName.Empty",
        "Full name cannot be empty");

    public static readonly Error TooLong = Error.Validation(
        "EmployeeFullName.TooLong",
        $"Full name cannot exceed {EmployeeFullName.MaxLength} characters");
}
