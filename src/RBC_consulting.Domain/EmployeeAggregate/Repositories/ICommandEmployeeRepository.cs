using RBC_consulting.Domain.Common.Repositories;
using RBC_consulting.Domain.EmployeeAggregate.Ids;

namespace RBC_consulting.Domain.EmployeeAggregate.Repositories;

public interface ICommandEmployeeRepository
    : IBaseCommandRepository<Employee, EmployeeId>;
