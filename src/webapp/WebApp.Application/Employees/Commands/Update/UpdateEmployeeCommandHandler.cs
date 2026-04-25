using WebApp.Application.Common.CQRS;
using WebApp.Application.Employees.Commands.Сommon.Validators;
using WebApp.Domain.Common.Results;
using WebApp.Domain.EmployeeAggregate.Ids;
using WebApp.Domain.EmployeeAggregate.Repositories;

namespace WebApp.Application.Employees.Commands.Update;

internal sealed class UpdateEmployeeCommandHandler(ICommandEmployeeRepository repository)
    : ICommandHandler<UpdateEmployeeCommand>
{
    public async Task<Result> Handle(UpdateEmployeeCommand request, CancellationToken cancellationToken)
    {
        var employeeResult = await repository.GetAsync(new EmployeeId(request.Id), cancellationToken);
        if (employeeResult.IsFailure)
            return Result.Failure(employeeResult.Error);

        var valueObjects = EmployeeValueObjects.From(request);
        if (valueObjects.IsFailure)
            return Result.Failure(valueObjects.Error);

        var vo = valueObjects.Value;
        var updateResult = employeeResult.Value.Update(
            vo.FullName,
            vo.Position,
            vo.Department,
            request.HireDate,
            vo.Email,
            vo.Phone,
            vo.Salary);

        if (updateResult.IsFailure)
            return updateResult;

        return await repository.UpdateAsync(employeeResult.Value, cancellationToken);
    }
}
