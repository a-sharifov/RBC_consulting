namespace RBC_consulting.Contracts.File;

public interface IFileService
{
    Task<SavedFile> SaveFileAsync(Stream fileStream, string fileName);
    Task DeleteFileAsync(string? filePath);
    Task<FileContent?> GetFileAsync(string filePath);
    FileContent BuildFileContent(byte[] data, string filePath);
}
