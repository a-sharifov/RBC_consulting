using RBC_consulting.Application.Common.CQRS;
using RBC_consulting.Contracts.File;

namespace RBC_consulting.Application.Employees.Queries.GetFile;

public sealed record GetEmployeeFileQuery(int EmployeeId) : IQuery<FileContent>;
