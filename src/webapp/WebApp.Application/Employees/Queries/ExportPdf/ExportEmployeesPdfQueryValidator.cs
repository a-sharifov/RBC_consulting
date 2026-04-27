using FluentValidation;
using WebApp.Domain.EmployeeAggregate.Enumerations;

namespace WebApp.Application.Employees.Queries.ExportPdf;

public sealed class ExportEmployeesPdfQueryValidator : AbstractValidator<ExportEmployeesPdfQuery>
{
    private static readonly string[] ValidSortDirs = ["asc", "desc"];

    public ExportEmployeesPdfQueryValidator()
    {
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
