using RBC_consulting.Application.Common.CQRS;
using RBC_consulting.Application.Employees.Commands.Сommon;

namespace RBC_consulting.Application.Employees.Commands.Update;

public sealed record UpdateEmployeeCommand(
    int Id,
    string FullName,
    string Position,
    string Department,
    DateTime HireDate,
    string? Email,
    string? Phone,
    decimal? Salary) : ICommand, IEmployeeFields;
