using FluentValidation;

namespace RBC_consulting.Application.Employees.Commands.ClearFile;

public sealed class ClearEmployeeFileCommandValidator : AbstractValidator<ClearEmployeeFileCommand>
{
    public ClearEmployeeFileCommandValidator()
    {
        RuleFor(c => c.EmployeeId)
            .GreaterThan(0)
            .WithMessage("Employee ID must be greater than 0");
    }
}
