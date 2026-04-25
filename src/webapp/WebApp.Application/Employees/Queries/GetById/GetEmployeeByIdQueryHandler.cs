using WebApp.Application.Common.CQRS;
using WebApp.Domain.Common.Results;
using WebApp.Domain.EmployeeAggregate.Ids;
using WebApp.Domain.EmployeeAggregate.Repositories;

namespace WebApp.Application.Employees.Queries.GetById;

internal sealed class GetEmployeeByIdQueryHandler(IQueryEmployeeRepository repository)
    : IQueryHandler<GetEmployeeByIdQuery, GetEmployeeByIdResponse>
{
    public async Task<Result<GetEmployeeByIdResponse>> Handle(GetEmployeeByIdQuery request, CancellationToken cancellationToken)
    {
        var result = await repository.GetAsync(new EmployeeId(request.Id));
        if (result.IsFailure)
            return Result.Failure<GetEmployeeByIdResponse>(result.Error);

        var p = result.Value;
        return Result.Success(new GetEmployeeByIdResponse(
            p.Id, p.FullName, p.Position, p.Department,
            p.HireDate, p.Email, p.Phone, p.Salary,
            p.CreatedAt, p.FilePath, p.HasFile));
    }
}
