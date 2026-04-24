namespace RBC_consulting.Domain.EmployeeAggregate.Projections;

public sealed record EmployeeFileData(string? FilePath, byte[]? FileBlob);
