using RBC_consulting.Application.Common.CQRS;

namespace RBC_consulting.Application.Employees.Queries.GetStatistics;

public sealed record GetEmployeesStatisticsQuery() : IQuery<IEnumerable<GetEmployeesStatisticsResponse>>;
