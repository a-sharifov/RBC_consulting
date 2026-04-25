using WebApp.Application.Common.CQRS;
using WebApp.Application.Employees.Commands.Сommon.Validators;
using WebApp.Domain.Common.Results;
using WebApp.Domain.EmployeeAggregate;
using WebApp.Domain.EmployeeAggregate.Repositories;

namespace WebApp.Application.Employees.Commands.Create;

internal sealed class CreateEmployeeCommandHandler(ICommandEmployeeRepository repository)
    : ICommandHandler<CreateEmployeeCommand, int>
{
    public async Task<Result<int>> Handle(CreateEmployeeCommand request, CancellationToken cancellationToken)
    {
        var valueObjects = EmployeeValueObjects.From(request);
        if (valueObjects.IsFailure)
            return Result.Failure<int>(valueObjects.Error);

        var vo = valueObjects.Value;

        var employee = Employee.Create(
            vo.FullName,
            vo.Position,
            vo.Department,
            request.HireDate,
            vo.Email,
            vo.Phone,
            vo.Salary,
            DateTime.UtcNow);

        var added = await repository.AddAsync(employee.Value, cancellationToken);
        return added.IsFailure
            ? Result.Failure<int>(added.Error)
            : Result.Success(added.Value.Value);
    }
}
