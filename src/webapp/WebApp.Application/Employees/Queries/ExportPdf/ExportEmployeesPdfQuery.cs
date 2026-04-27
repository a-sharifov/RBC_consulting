using WebApp.Application.Common.CQRS;

namespace WebApp.Application.Employees.Queries.ExportPdf;

public sealed record ExportEmployeesPdfQuery(
    string? SearchTerm = null,
    string? SortBy = null,
    string? SortDir = null) : IQuery<ExportEmployeesPdfResponse>;
