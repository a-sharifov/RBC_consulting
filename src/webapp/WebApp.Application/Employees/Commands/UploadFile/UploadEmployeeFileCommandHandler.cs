using WebApp.Application.Common.CQRS;
using WebApp.Contracts.File;
using WebApp.Domain.Common.Errors;
using WebApp.Domain.Common.Results;
using WebApp.Domain.EmployeeAggregate.Ids;
using WebApp.Domain.EmployeeAggregate.Repositories;

namespace WebApp.Application.Employees.Commands.UploadFile;

internal sealed class UploadEmployeeFileCommandHandler(
    ICommandEmployeeRepository repository,
    IFileService fileService) : ICommandHandler<UploadEmployeeFileCommand>
{
    public async Task<Result> Handle(UploadEmployeeFileCommand request, CancellationToken cancellationToken)
    {
        var employeeResult = await repository.GetAsync(new EmployeeId(request.EmployeeId), cancellationToken);
        if (employeeResult.IsFailure)
            return Result.Failure(employeeResult.Error);

        var employee = employeeResult.Value;

        if (!string.IsNullOrEmpty(employee.FilePath))
            await fileService.DeleteFileAsync(employee.FilePath);

        try
        {
            var saved = await fileService.SaveFileAsync(request.FileStream, request.FileName);
            employee.SetFile(saved.Blob, saved.RelativePath);
            return await repository.UpdateAsync(employee, cancellationToken);
        }
        catch (Exception ex)
        {
            return Result.Failure(Error.Failure("File.SaveError", ex.Message));
        }
    }
}
