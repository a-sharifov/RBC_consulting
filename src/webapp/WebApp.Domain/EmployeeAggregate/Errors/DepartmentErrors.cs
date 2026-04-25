using WebApp.Domain.Common.Errors;
using WebApp.Domain.EmployeeAggregate.ValueObjects;

namespace WebApp.Domain.EmployeeAggregate.Errors;

public static class DepartmentErrors
{
    public static readonly Error Empty = Error.Validation(
        "Department.Empty",
        "Department cannot be empty");

    public static readonly Error TooLong = Error.Validation(
        "Department.TooLong",
        $"Department cannot exceed {Department.MaxLength} characters");
}
