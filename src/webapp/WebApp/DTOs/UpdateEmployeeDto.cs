namespace WebApp.Api.DTOs;

public sealed record UpdateEmployeeDto(
    string FullName,
    string Position,
    string Department,
    DateTime HireDate,
    string? Email,
    string? Phone,
    decimal? Salary);
