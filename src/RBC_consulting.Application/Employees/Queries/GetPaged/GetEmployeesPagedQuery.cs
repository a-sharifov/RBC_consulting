using RBC_consulting.Application.Common.CQRS;
using RBC_consulting.Domain.Common.Paginations;

namespace RBC_consulting.Application.Employees.Queries.GetPaged;

public sealed record GetEmployeesPagedQuery(
    int PageNumber = 1,
    int PageSize = 10,
    string? SearchTerm = null,
    string? SortBy = null,
    string? SortDir = null) : IQuery<PagedList<GetEmployeesPagedResponse>>;
