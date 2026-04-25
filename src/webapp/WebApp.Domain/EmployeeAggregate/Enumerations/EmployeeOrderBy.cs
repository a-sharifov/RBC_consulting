using WebApp.Domain.Common.Enumerations;

namespace WebApp.Domain.EmployeeAggregate.Enumerations;

public sealed class EmployeeOrderBy(int value, string name) : Enumeration<EmployeeOrderBy>(value, name)
{
    public static readonly EmployeeOrderBy EmployeeID = new(1, nameof(EmployeeID));
    public static readonly EmployeeOrderBy FullName = new(2, nameof(FullName));
    public static readonly EmployeeOrderBy Position = new(3, nameof(Position));
    public static readonly EmployeeOrderBy Department = new(4, nameof(Department));
    public static readonly EmployeeOrderBy HireDate = new(5, nameof(HireDate));
    public static readonly EmployeeOrderBy Email = new(6, nameof(Email));
    public static readonly EmployeeOrderBy Phone = new(7, nameof(Phone));
    public static readonly EmployeeOrderBy Salary = new(8, nameof(Salary));
}
