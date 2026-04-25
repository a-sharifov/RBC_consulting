using WebApp.Application.Common.CQRS;
using WebApp.Application.Employees.Commands.Сommon;

namespace WebApp.Application.Employees.Commands.Update;

public sealed record UpdateEmployeeCommand(
    int Id,
    string FullName,
    string Position,
    string Department,
    DateTime HireDate,
    string? Email,
    string? Phone,
    decimal? Salary) : ICommand, IEmployeeFields;
