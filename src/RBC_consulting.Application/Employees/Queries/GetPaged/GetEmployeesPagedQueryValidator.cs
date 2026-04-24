using FluentValidation;
using RBC_consulting.Domain.EmployeeAggregate.Enumerations;

namespace RBC_consulting.Application.Employees.Queries.GetPaged;

public sealed class GetEmployeesPagedQueryValidator : AbstractValidator<GetEmployeesPagedQuery>
{
    private static readonly string[] ValidSortDirs = ["asc", "desc"];

    public GetEmployeesPagedQueryValidator()
    {
        RuleFor(q => q.PageNumber)
            .GreaterThan(0)
            .WithMessage("Page number must be greater than 0");

        RuleFor(q => q.PageSize)
            .GreaterThan(0)
            .LessThanOrEqualTo(100)
            .WithMessage("Page size must be between 1 and 100");

        RuleFor(q => q.SortBy)
            .Must(sortBy => sortBy is null || EmployeeOrderBy.NameExists(sortBy))
            .WithMessage($"Sort field must be one of: {string.Join(", ", EmployeeOrderBy.GetNames())}");

        RuleFor(q => q.SortDir)
            .Must(sortDir => sortDir is null || ValidSortDirs.Contains(sortDir.ToLowerInvariant()))
            .WithMessage("Sort direction must be 'asc' or 'desc'");

        RuleFor(q => q.SearchTerm)
            .MaximumLength(200)
            .WithMessage("Search term cannot exceed 200 characters");
    }
}
