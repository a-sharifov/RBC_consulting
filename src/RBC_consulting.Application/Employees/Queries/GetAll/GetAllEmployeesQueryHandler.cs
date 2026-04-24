using RBC_consulting.Application.Common.CQRS;
using RBC_consulting.Domain.Common.Results;
using RBC_consulting.Domain.EmployeeAggregate.Repositories;

namespace RBC_consulting.Application.Employees.Queries.GetAll;

internal sealed class GetAllEmployeesQueryHandler(IQueryEmployeeRepository repository)
    : IQueryHandler<GetAllEmployeesQuery, IEnumerable<GetAllEmployeesResponse>>
{
    public async Task<Result<IEnumerable<GetAllEmployeesResponse>>> Handle(GetAllEmployeesQuery request, CancellationToken cancellationToken)
    {
        var result = await repository.GetAllForExportAsync(request.SearchTerm, request.SortBy, request.SortDir);
        if (result.IsFailure)
            return Result.Failure<IEnumerable<GetAllEmployeesResponse>>(result.Error);

        var mapped = result.Value.Select(p => new GetAllEmployeesResponse(
            p.Id, p.FullName, p.Position, p.Department,
            p.HireDate, p.Email, p.Phone, p.Salary,
            p.CreatedAt, p.FilePath, p.HasFile));

        return Result.Success(mapped);
    }
}
