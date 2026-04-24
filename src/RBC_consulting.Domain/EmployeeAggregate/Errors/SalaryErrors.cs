using RBC_consulting.Domain.Common.Errors;
using RBC_consulting.Domain.EmployeeAggregate.ValueObjects;

namespace RBC_consulting.Domain.EmployeeAggregate.Errors;

public static class SalaryErrors
{
    public static readonly Error Negative = Error.Validation(
        "Salary.Negative",
        "Salary cannot be negative");

    public static readonly Error TooHigh = Error.Validation(
        "Salary.TooHigh",
        $"Salary cannot exceed {Salary.MaxValue}");
}
