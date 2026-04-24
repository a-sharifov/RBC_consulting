using RBC_consulting.Application.Common.CQRS;
using RBC_consulting.Application.Employees.Commands.Сommon;

namespace RBC_consulting.Application.Employees.Commands.Create
{
    public sealed record CreateEmployeeCommand(
        string FullName,
        string Position,
        string Department,
        DateTime HireDate,
        string? Email,
        string? Phone,
        decimal? Salary) : ICommand<int>, IEmployeeFields;
}
