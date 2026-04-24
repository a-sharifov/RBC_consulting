using RBC_consulting.Application.Common.CQRS;

namespace RBC_consulting.Application.Employees.Queries.GetById;

public sealed record GetEmployeeByIdQuery(int Id) : IQuery<GetEmployeeByIdResponse>;
