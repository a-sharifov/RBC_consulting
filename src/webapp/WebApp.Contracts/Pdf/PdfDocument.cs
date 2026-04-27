namespace WebApp.Contracts.Pdf;

public sealed record PdfDocument(byte[] Data, string ContentType, string FileName);
