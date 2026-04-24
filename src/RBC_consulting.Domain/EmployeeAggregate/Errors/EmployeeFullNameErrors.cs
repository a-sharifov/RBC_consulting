using RBC_consulting.Domain.Common.Errors;
using RBC_consulting.Domain.EmployeeAggregate.ValueObjects;

namespace RBC_consulting.Domain.EmployeeAggregate.Errors;

public static class EmployeeFullNameErrors
{
    public static readonly Error Empty = Error.Validation(
        "EmployeeFullName.Empty",
        "Full name cannot be empty");

    public static readonly Error TooLong = Error.Validation(
        "EmployeeFullName.TooLong",
        $"Full name cannot exceed {EmployeeFullName.MaxLength} characters");
}
