using WebApp.Application.Common.CQRS;

namespace WebApp.Application.Employees.Queries.GetAll;

public sealed record GetAllEmployeesQuery(
    string? SearchTerm = null,
    string? SortBy = null,
    string? SortDir = null) : IQuery<IEnumerable<GetAllEmployeesResponse>>;
