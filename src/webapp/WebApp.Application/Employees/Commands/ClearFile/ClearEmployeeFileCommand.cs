using WebApp.Application.Common.CQRS;

namespace WebApp.Application.Employees.Commands.ClearFile;

public sealed record ClearEmployeeFileCommand(int EmployeeId) : ICommand;
