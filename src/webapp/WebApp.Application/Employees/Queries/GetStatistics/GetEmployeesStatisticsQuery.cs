using WebApp.Application.Common.CQRS;

namespace WebApp.Application.Employees.Queries.GetStatistics;

public sealed record GetEmployeesStatisticsQuery() : IQuery<IEnumerable<GetEmployeesStatisticsResponse>>;
