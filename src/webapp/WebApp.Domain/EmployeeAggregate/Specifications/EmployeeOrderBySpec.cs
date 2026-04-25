using WebApp.Domain.EmployeeAggregate.Enumerations;

namespace WebApp.Domain.EmployeeAggregate.Specifications;

public sealed class EmployeeOrderBySpec : Specification<Employee>
{
    public EmployeeOrderBySpec(EmployeeOrderBy? orderBy, bool isDescending)
    {
        switch (orderBy?.Name)
        {
            case nameof(EmployeeOrderBy.EmployeeID):
                Query.OrderBy(e => e.Id.Value, isDescending);
                break;
            case nameof(EmployeeOrderBy.FullName):
                Query.OrderBy(e => e.FullName, isDescending);
                break;
            case nameof(EmployeeOrderBy.Position):
                Query.OrderBy(e => e.Position, isDescending);
                break;
            case nameof(EmployeeOrderBy.Department):
                Query.OrderBy(e => e.Department, isDescending);
                break;
            case nameof(EmployeeOrderBy.HireDate):
                Query.OrderBy(e => e.HireDate, isDescending);
                break;
            case nameof(EmployeeOrderBy.Email):
                Query.OrderBy(e => e.Email, isDescending);
                break;
            case nameof(EmployeeOrderBy.Phone):
                Query.OrderBy(e => e.Phone, isDescending);
                break;
            case nameof(EmployeeOrderBy.Salary):
                Query.OrderBy(e => e.Salary, isDescending);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(orderBy), orderBy, null);
        }
    }
}
