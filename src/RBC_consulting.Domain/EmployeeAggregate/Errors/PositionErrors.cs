using RBC_consulting.Domain.Common.Errors;
using RBC_consulting.Domain.EmployeeAggregate.ValueObjects;

namespace RBC_consulting.Domain.EmployeeAggregate.Errors;

public static class PositionErrors
{
    public static readonly Error Empty = Error.Validation(
        "Position.Empty",
        "Position cannot be empty");

    public static readonly Error TooLong = Error.Validation(
        "Position.TooLong",
        $"Position cannot exceed {Position.MaxLength} characters");
}
