using WebApp.Application.Common.CQRS;
using WebApp.Domain.Common.Results;
using WebApp.Domain.EmployeeAggregate.Repositories;

namespace WebApp.Application.Employees.Queries.GetStatistics;

internal sealed class GetEmployeesStatisticsQueryHandler(IQueryEmployeeRepository repository)
    : IQueryHandler<GetEmployeesStatisticsQuery, IEnumerable<GetEmployeesStatisticsResponse>>
{
    public async Task<Result<IEnumerable<GetEmployeesStatisticsResponse>>> Handle(GetEmployeesStatisticsQuery request, CancellationToken cancellationToken)
    {
        var result = await repository.GetStatisticsAsync();
        if (result.IsFailure)
            return Result.Failure<IEnumerable<GetEmployeesStatisticsResponse>>(result.Error);

        var mapped = result.Value.Select(s => new GetEmployeesStatisticsResponse(s.Department, s.EmployeeCount));
        return Result.Success(mapped);
    }
}
