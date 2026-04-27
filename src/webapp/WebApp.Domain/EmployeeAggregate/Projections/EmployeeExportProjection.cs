namespace WebApp.Domain.EmployeeAggregate.Projections;

public sealed class EmployeeExportProjection
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Position { get; set; } = string.Empty;
    public string Department { get; set; } = string.Empty;
    public DateTime HireDate { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public decimal? Salary { get; set; }
}
