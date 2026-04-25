using WebApp.Application.Common.CQRS;
using WebApp.Contracts.File;
using WebApp.Domain.Common.Results;
using WebApp.Domain.EmployeeAggregate.Ids;
using WebApp.Domain.EmployeeAggregate.Repositories;

namespace WebApp.Application.Employees.Commands.Delete;

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
