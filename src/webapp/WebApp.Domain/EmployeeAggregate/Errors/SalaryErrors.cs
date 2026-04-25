using WebApp.Domain.Common.Errors;
using WebApp.Domain.EmployeeAggregate.ValueObjects;

namespace WebApp.Domain.EmployeeAggregate.Errors;

public static class SalaryErrors
{
    public static readonly Error Negative = Error.Validation(
        "Salary.Negative",
        "Salary cannot be negative");

    public static readonly Error TooHigh = Error.Validation(
        "Salary.TooHigh",
        $"Salary cannot exceed {Salary.MaxValue}");
}
