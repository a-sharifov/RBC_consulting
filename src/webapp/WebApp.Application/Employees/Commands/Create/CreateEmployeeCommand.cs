using WebApp.Application.Common.CQRS;
using WebApp.Application.Employees.Commands.Сommon;

namespace WebApp.Application.Employees.Commands.Create
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
