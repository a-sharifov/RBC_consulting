using WebApp.Domain.Common.Repositories;
using WebApp.Domain.EmployeeAggregate.Ids;

namespace WebApp.Domain.EmployeeAggregate.Repositories;

public interface ICommandEmployeeRepository
    : IBaseCommandRepository<Employee, EmployeeId>;
