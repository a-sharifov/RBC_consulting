namespace WebApp.Contracts.Pdf;

public sealed record EmployeePdfRow(
    int Id,
    string FullName,
    string Position,
    string Department,
    DateTime HireDate,
    string? Email,
    string? Phone,
    decimal? Salary);
