namespace WebApp.Application.Employees.Queries.GetPaged;

public sealed record GetEmployeesPagedResponse(
    int Id,
    string FullName,
    string Position,
    string Department,
    DateTime HireDate,
    string? Email,
    string? Phone,
    decimal? Salary,
    DateTime CreatedAt,
    string? FilePath,
    bool HasFile);
