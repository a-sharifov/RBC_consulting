namespace RBC_consulting.Domain.EmployeeAggregate.Projections;

public sealed class EmployeeDepartmentStat
{
    public string Department { get; set; } = string.Empty;
    public int EmployeeCount { get; set; }
}
