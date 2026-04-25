using WebApp.Application.Common.CQRS;
using WebApp.Contracts.File;
using WebApp.Domain.Common.Errors;
using WebApp.Domain.Common.Results;
using WebApp.Domain.EmployeeAggregate.Ids;
using WebApp.Domain.EmployeeAggregate.Repositories;

namespace WebApp.Application.Employees.Queries.GetFile;

internal sealed class GetEmployeeFileQueryHandler(
    IQueryEmployeeRepository repository,
    IFileService fileService) : IQueryHandler<GetEmployeeFileQuery, FileContent>
{
    private static readonly Error NotFound =
        Error.NotFound("File.NotFound", "The requested file was not found.");

    public async Task<Result<FileContent>> Handle(GetEmployeeFileQuery request, CancellationToken cancellationToken)
    {
        var filePathResult = await repository.GetFilePathAsync(new EmployeeId(request.EmployeeId));

        if (filePathResult.IsFailure)
            return Result.Failure<FileContent>(filePathResult.Error);

        var filePath = filePathResult.Value;

        if (string.IsNullOrEmpty(filePath))
            return Result.Failure<FileContent>(NotFound);

        var fromDisk = await fileService.GetFileAsync(filePath);

        if (fromDisk is not null)
            return Result.Success(fromDisk);

        var fileDataResult = await repository.GetFileAsync(new EmployeeId(request.EmployeeId));
        if (fileDataResult.IsFailure)
            return Result.Failure<FileContent>(fileDataResult.Error);

        var fileBlob = fileDataResult.Value.FileBlob;
        if (fileBlob is { Length: > 0 })
            return Result.Success(fileService.BuildFileContent(fileBlob, filePath));

        return Result.Failure<FileContent>(NotFound);
    }
}
