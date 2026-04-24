using RBC_consulting.Application.Common.CQRS;
using RBC_consulting.Domain.Common.Results;
using RBC_consulting.Domain.EmployeeAggregate.Ids;
using RBC_consulting.Domain.EmployeeAggregate.Repositories;

namespace RBC_consulting.Application.Employees.Queries.GetById;

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
