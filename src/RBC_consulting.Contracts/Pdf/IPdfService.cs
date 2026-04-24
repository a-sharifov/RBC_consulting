namespace RBC_consulting.Contracts.Pdf;

public interface IPdfService
{
    Task<byte[]> GenerateEmployeesPdfAsync(IEnumerable<EmployeePdfRow> rows);
}
