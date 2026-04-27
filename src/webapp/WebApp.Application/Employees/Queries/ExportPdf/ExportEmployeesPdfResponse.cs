namespace WebApp.Application.Employees.Queries.ExportPdf;

public sealed record ExportEmployeesPdfResponse(
    byte[] Data,
    string ContentType,
    string FileName);
