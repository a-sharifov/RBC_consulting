namespace RBC_consulting.Domain.EmployeeAggregate.Projections;

public sealed class EmployeeProjection
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Position { get; set; } = string.Empty;
    public string Department { get; set; } = string.Empty;
    public DateTime HireDate { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public decimal? Salary { get; set; }
    public DateTime CreatedAt { get; set; }
    public string? FilePath { get; set; }
    public bool HasFile { get; set; }
}
