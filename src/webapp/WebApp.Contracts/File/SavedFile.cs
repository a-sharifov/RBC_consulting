namespace WebApp.Contracts.File;

public sealed record SavedFile(byte[] Blob, string RelativePath);
