using FluentValidation;
using RBC_consulting.Application.Employees.Commands.Сommon.Validators;

namespace RBC_consulting.Application.Employees.Commands.Update;

public sealed class UpdateEmployeeCommandValidator : EmployeeFieldsValidator<UpdateEmployeeCommand>
{
    public UpdateEmployeeCommandValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
    }
}
