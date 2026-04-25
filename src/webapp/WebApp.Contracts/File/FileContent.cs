namespace WebApp.Contracts.File;

public sealed record FileContent(byte[] Data, string ContentType, string FileName);
