using RBC_consulting.Application.Common.CQRS;
using RBC_consulting.Contracts.File;
using RBC_consulting.Domain.Common.Results;
using RBC_consulting.Domain.EmployeeAggregate.Ids;
using RBC_consulting.Domain.EmployeeAggregate.Repositories;

namespace RBC_consulting.Application.Employees.Commands.ClearFile;

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
