using WebApp.Application.Common.CQRS;
using WebApp.Contracts.Pdf;
using WebApp.Domain.Common.Results;
using WebApp.Domain.EmployeeAggregate.Repositories;

namespace WebApp.Application.Employees.Queries.ExportPdf;

internal sealed class ExportEmployeesPdfQueryHandler(
    IQueryEmployeeRepository repository,
    IPdfService pdfService) : IQueryHandler<ExportEmployeesPdfQuery, ExportEmployeesPdfResponse>
{
    public async Task<Result<ExportEmployeesPdfResponse>> Handle(ExportEmployeesPdfQuery request, CancellationToken cancellationToken)
    {
        var result = await repository.GetForExportPdfAsync(request.SearchTerm, request.SortBy, request.SortDir);
        if (result.IsFailure)
            return Result.Failure<ExportEmployeesPdfResponse>(result.Error);

        var rows = result.Value.Select(p => new EmployeePdfRow(
            p.Id, p.FullName, p.Position, p.Department,
            p.HireDate, p.Email, p.Phone, p.Salary));

        var file = await pdfService.GenerateEmployeesPdfAsync(rows);

        return Result.Success(
            new ExportEmployeesPdfResponse(file.Data, file.ContentType, file.FileName));
    }
}
