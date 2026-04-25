namespace WebApp.Application.Employees.Commands.Сommon;

public interface IEmployeeFields
{
    string FullName { get; }
    string Position { get; }
    string Department { get; }
    DateTime HireDate { get; }
    string? Email { get; }
    string? Phone { get; }
    decimal? Salary { get; }
}
