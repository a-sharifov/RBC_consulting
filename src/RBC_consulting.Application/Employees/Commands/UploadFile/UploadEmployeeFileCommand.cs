using RBC_consulting.Application.Common.CQRS;

namespace RBC_consulting.Application.Employees.Commands.UploadFile;

public sealed record UploadEmployeeFileCommand(int EmployeeId, Stream FileStream, string FileName, long FileSize) : ICommand;
