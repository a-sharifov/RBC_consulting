using RBC_consulting.Application.Common.CQRS;

namespace RBC_consulting.Application.Employees.Commands.ClearFile;

public sealed record ClearEmployeeFileCommand(int EmployeeId) : ICommand;
