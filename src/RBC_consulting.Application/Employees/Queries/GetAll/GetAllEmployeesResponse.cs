namespace RBC_consulting.Application.Employees.Queries.GetAll;

public sealed record GetAllEmployeesResponse(
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
