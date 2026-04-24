namespace RBC_consulting.Contracts.File;

public sealed record SavedFile(byte[] Blob, string RelativePath);
