using WebApp.Application.Common.CQRS;
using WebApp.Domain.Common.Paginations;
using WebApp.Domain.Common.Results;
using WebApp.Domain.EmployeeAggregate.Repositories;

namespace WebApp.Application.Employees.Queries.GetPaged;

internal sealed class GetEmployeesPagedQueryHandler(IQueryEmployeeRepository repository)
    : IQueryHandler<GetEmployeesPagedQuery, PagedList<GetEmployeesPagedResponse>>
{
    public async Task<Result<PagedList<GetEmployeesPagedResponse>>> Handle(GetEmployeesPagedQuery request, CancellationToken cancellationToken)
    {
        var result = await repository.GetPagedAsync(
            request.PageNumber,
            request.PageSize,
            request.SearchTerm,
            request.SortBy,
            request.SortDir);

        if (result.IsFailure)
            return Result.Failure<PagedList<GetEmployeesPagedResponse>>(result.Error);

        var mapped = result.Value.Items.Select(p => new GetEmployeesPagedResponse(
            p.Id, p.FullName, p.Position, p.Department,
            p.HireDate, p.Email, p.Phone, p.Salary,
            p.CreatedAt, p.FilePath, p.HasFile));

        return Result.Success(new PagedList<GetEmployeesPagedResponse>(
            mapped, result.Value.TotalCount, result.Value.PageNumber, result.Value.PageSize));
    }
}
