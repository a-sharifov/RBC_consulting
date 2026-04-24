using RBC_consulting.Application.Common.CQRS;

namespace RBC_consulting.Application.Employees.Queries.GetAll;

public sealed record GetAllEmployeesQuery(
    string? SearchTerm = null,
    string? SortBy = null,
    string? SortDir = null) : IQuery<IEnumerable<GetAllEmployeesResponse>>;
