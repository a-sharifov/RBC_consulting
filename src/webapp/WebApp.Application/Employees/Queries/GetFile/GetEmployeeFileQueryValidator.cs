using FluentValidation;

namespace WebApp.Application.Employees.Queries.GetFile;

public sealed class GetEmployeeFileQueryValidator : AbstractValidator<GetEmployeeFileQuery>
{
    public GetEmployeeFileQueryValidator()
    {
        RuleFor(q => q.EmployeeId)
            .GreaterThan(0)
            .WithMessage("Employee ID must be greater than 0");
    }
}
