using WebApp.Application.Common.CQRS;
using WebApp.Contracts.File;

namespace WebApp.Application.Employees.Queries.GetFile;

public sealed record GetEmployeeFileQuery(int EmployeeId) : IQuery<FileContent>;
