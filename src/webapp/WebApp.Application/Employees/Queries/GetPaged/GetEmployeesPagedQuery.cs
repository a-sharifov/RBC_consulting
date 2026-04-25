using WebApp.Application.Common.CQRS;
using WebApp.Domain.Common.Paginations;

namespace WebApp.Application.Employees.Queries.GetPaged;

public sealed record GetEmployeesPagedQuery(
    int PageNumber = 1,
    int PageSize = 10,
    string? SearchTerm = null,
    string? SortBy = null,
    string? SortDir = null) : IQuery<PagedList<GetEmployeesPagedResponse>>;
