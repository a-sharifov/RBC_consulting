using WebApp.Application.Common.CQRS;

namespace WebApp.Application.Employees.Commands.Delete;

public sealed record DeleteEmployeeCommand(int Id) : ICommand;
