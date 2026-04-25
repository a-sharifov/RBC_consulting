using FluentValidation;

namespace WebApp.Application.Employees.Queries.GetById;

public sealed class GetEmployeeByIdQueryValidator : AbstractValidator<GetEmployeeByIdQuery>
{
    public GetEmployeeByIdQueryValidator()
    {
        RuleFor(q => q.Id)
            .GreaterThan(0)
            .WithMessage("Employee ID must be greater than 0");
    }
}
