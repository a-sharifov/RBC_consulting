using FluentValidation;

namespace WebApp.Application.Employees.Commands.Delete;

public sealed class DeleteEmployeeCommandValidator : AbstractValidator<DeleteEmployeeCommand>
{
    public DeleteEmployeeCommandValidator()
    {
        RuleFor(c => c.Id)
            .GreaterThan(0)
            .WithMessage("Employee ID must be greater than 0");
    }
}
