using WebApp.Application.Common.CQRS;
using WebApp.Contracts.File;
using WebApp.Domain.Common.Results;
using WebApp.Domain.EmployeeAggregate.Ids;
using WebApp.Domain.EmployeeAggregate.Repositories;

namespace WebApp.Application.Employees.Commands.ClearFile;

internal sealed class ClearEmployeeFileCommandHandler(
    ICommandEmployeeRepository repository,
    IFileService fileService) : ICommandHandler<ClearEmployeeFileCommand>
{
    public async Task<Result> Handle(ClearEmployeeFileCommand request, CancellationToken cancellationToken)
    {
        var employeeId = new EmployeeId(request.EmployeeId);
        var employeeResult = await repository.GetAsync(employeeId, cancellationToken);

        if (employeeResult.IsFailure)
            return employeeResult;

        var employee = employeeResult.Value;

        if (!string.IsNullOrEmpty(employee.FilePath))
        {
            await fileService.DeleteFileAsync(employee.FilePath);
            employee.ClearFile();
            return await repository.UpdateAsync(employee, cancellationToken);
        }

        return Result.Success();
    }
}
