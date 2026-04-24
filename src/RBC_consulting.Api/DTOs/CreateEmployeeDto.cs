namespace RBC_consulting.DTOs;

public sealed record CreateEmployeeDto(
    string FullName,
    string Position,
    string Department,
    DateTime HireDate,
    string? Email,
    string? Phone,
    decimal? Salary);
