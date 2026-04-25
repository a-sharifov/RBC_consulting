using WebApp.Application.Common.CQRS;

namespace WebApp.Application.Employees.Commands.UploadFile;

public sealed record UploadEmployeeFileCommand(int EmployeeId, Stream FileStream, string FileName, long FileSize) : ICommand;
