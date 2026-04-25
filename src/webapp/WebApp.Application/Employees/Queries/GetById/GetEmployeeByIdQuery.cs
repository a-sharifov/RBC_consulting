using WebApp.Application.Common.CQRS;

namespace WebApp.Application.Employees.Queries.GetById;

public sealed record GetEmployeeByIdQuery(int Id) : IQuery<GetEmployeeByIdResponse>;
