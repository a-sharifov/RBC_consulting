using RBC_consulting.Application.Common.CQRS;
using RBC_consulting.Contracts.File;
using RBC_consulting.Domain.Common.Results;
using RBC_consulting.Domain.EmployeeAggregate.Ids;
using RBC_consulting.Domain.EmployeeAggregate.Repositories;

namespace RBC_consulting.Application.Employees.Commands.Delete;

internal sealed class DeleteEmployeeCommandHandler(
    ICommandEmployeeRepository repository,
    IFileService fileService) : ICommandHandler<DeleteEmployeeCommand>
{
    public async Task<Result> Handle(DeleteEmployeeCommand request, CancellationToken cancellationToken)
    {
        var employeeId = new EmployeeId(request.Id);
        var employeeResult = await repository.GetAsync(employeeId, cancellationToken);

        if (employeeResult.IsFailure)
            return employeeResult;

        var employee = employeeResult.Value;

        if (!string.IsNullOrEmpty(employee.FilePath))
            await fileService.DeleteFileAsync(employee.FilePath);

        return await repository.DeleteAsync(employeeId, cancellationToken);
    }
}
