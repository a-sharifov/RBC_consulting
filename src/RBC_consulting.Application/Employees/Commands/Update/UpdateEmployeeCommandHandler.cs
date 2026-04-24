using RBC_consulting.Application.Common.CQRS;
using RBC_consulting.Application.Employees.Commands.Сommon.Validators;
using RBC_consulting.Domain.Common.Results;
using RBC_consulting.Domain.EmployeeAggregate.Ids;
using RBC_consulting.Domain.EmployeeAggregate.Repositories;

namespace RBC_consulting.Application.Employees.Commands.Update;

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
