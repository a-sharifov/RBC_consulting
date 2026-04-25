namespace WebApp.Domain.EmployeeAggregate.Projections;

public sealed record EmployeeFileData(string? FilePath, byte[]? FileBlob);
