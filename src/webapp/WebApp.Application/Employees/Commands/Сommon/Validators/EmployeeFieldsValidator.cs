using FluentValidation;
using WebApp.Domain.EmployeeAggregate.ValueObjects;

namespace WebApp.Application.Employees.Commands.Сommon.Validators;

public class EmployeeFieldsValidator<T> : AbstractValidator<T> where T : IEmployeeFields
{
    public EmployeeFieldsValidator()
    {
        RuleFor(x => x.FullName).NotEmpty().MaximumLength(EmployeeFullName.MaxLength);
        RuleFor(x => x.Position).NotEmpty().MaximumLength(Position.MaxLength);
        RuleFor(x => x.Department).NotEmpty().MaximumLength(Department.MaxLength);
        RuleFor(x => x.HireDate).LessThanOrEqualTo(_ => DateTime.UtcNow).WithMessage("Hire date cannot be in the future");
        RuleFor(x => x.Email).EmailAddress().When(x => !string.IsNullOrEmpty(x.Email));
        RuleFor(x => x.Phone).MaximumLength(Phone.MaxLength).When(x => !string.IsNullOrEmpty(x.Phone));
        RuleFor(x => x.Salary).GreaterThanOrEqualTo(0).When(x => x.Salary.HasValue);
    }
}
