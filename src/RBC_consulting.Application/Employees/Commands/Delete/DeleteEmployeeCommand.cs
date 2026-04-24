using RBC_consulting.Application.Common.CQRS;

namespace RBC_consulting.Application.Employees.Commands.Delete;

public sealed record DeleteEmployeeCommand(int Id) : ICommand;
