using FluentValidation;
using WebApp.Application.Employees.Commands.Сommon.Validators;

namespace WebApp.Application.Employees.Commands.Update;

public sealed class UpdateEmployeeCommandValidator : EmployeeFieldsValidator<UpdateEmployeeCommand>
{
    public UpdateEmployeeCommandValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
    }
}
