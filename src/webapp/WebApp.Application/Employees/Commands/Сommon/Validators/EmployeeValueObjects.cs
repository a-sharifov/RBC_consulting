using WebApp.Application.Employees.Commands.Сommon;
using WebApp.Domain.Common.Results;
using VO = WebApp.Domain.EmployeeAggregate.ValueObjects;

namespace WebApp.Application.Employees.Commands.Сommon.Validators;

internal sealed record EmployeeValueObjects(
    VO.EmployeeFullName FullName,
    VO.Position Position,
    VO.Department Department,
    VO.Email? Email,
    VO.Phone? Phone,
    VO.Salary? Salary)
{
    public static Result<EmployeeValueObjects> From(IEmployeeFields fields)
    {
        var fullName = VO.EmployeeFullName.Create(fields.FullName);
        if (fullName.IsFailure) return Result.Failure<EmployeeValueObjects>(fullName.Error);

        var position = VO.Position.Create(fields.Position);
        if (position.IsFailure) return Result.Failure<EmployeeValueObjects>(position.Error);

        var department = VO.Department.Create(fields.Department);
        if (department.IsFailure) return Result.Failure<EmployeeValueObjects>(department.Error);

        VO.Email? email = null;
        if (fields.Email is not null)
        {
            var created = VO.Email.Create(fields.Email);
            if (created.IsFailure) return Result.Failure<EmployeeValueObjects>(created.Error);
            email = created.Value;
        }

        VO.Phone? phone = null;
        if (fields.Phone is not null)
        {
            var created = VO.Phone.Create(fields.Phone);
            if (created.IsFailure) return Result.Failure<EmployeeValueObjects>(created.Error);
            phone = created.Value;
        }

        VO.Salary? salary = null;
        if (fields.Salary.HasValue)
        {
            var created = VO.Salary.Create(fields.Salary.Value);
            if (created.IsFailure) return Result.Failure<EmployeeValueObjects>(created.Error);
            salary = created.Value;
        }

        return Result.Success(new EmployeeValueObjects(
            fullName.Value, position.Value, department.Value, email, phone, salary));
    }
}
