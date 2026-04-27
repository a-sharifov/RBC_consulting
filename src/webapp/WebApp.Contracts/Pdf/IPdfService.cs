namespace WebApp.Contracts.Pdf;

public interface IPdfService
{
    Task<PdfDocument> GenerateEmployeesPdfAsync(IEnumerable<EmployeePdfRow> rows);
}
