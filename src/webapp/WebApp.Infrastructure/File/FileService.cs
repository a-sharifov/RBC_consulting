using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using WebApp.Contracts.File;

namespace WebApp.Infrastructure.File;

internal sealed class FileService : IFileService
{
    private readonly FileSettings _settings;
    private readonly string _uploadFolder;
    private readonly ILogger<FileService> _logger;

    public FileService(IOptions<FileSettings> fileSettings, ILogger<FileService> logger)
    {
        _settings = fileSettings.Value;
        _logger = logger;
        _uploadFolder = Path.Combine(_settings.WebRootPath, _settings.UploadFolderName);

        if (!string.IsNullOrEmpty(_settings.WebRootPath) && !Directory.Exists(_uploadFolder))
            Directory.CreateDirectory(_uploadFolder);
    }

    public async Task<SavedFile> SaveFileAsync(Stream fileStream, string fileName)
    {
        var extension = Path.GetExtension(fileName).ToLowerInvariant();
        if (!_settings.AllowedExtensions.Contains(extension))
            throw new InvalidOperationException($"File type {extension} is not allowed");

        var uniqueFileName = $"{Guid.NewGuid()}{extension}";
        var relativePath = $"{_settings.UploadFolderName}/{uniqueFileName}";

        using var ms = new MemoryStream();
        await fileStream.CopyToAsync(ms);
        var blob = ms.ToArray();

        try
        {
            await System.IO.File.WriteAllBytesAsync(Path.Combine(_uploadFolder, uniqueFileName), blob);
            _logger.LogInformation("File saved to disk: {RelativePath}", relativePath);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to write file to disk, blob stored in DB only: {FileName}", uniqueFileName);
        }

        return new SavedFile(blob, relativePath);
    }

    public Task DeleteFileAsync(string? filePath)
    {
        if (string.IsNullOrWhiteSpace(filePath))
            return Task.CompletedTask;

        var fullPath = Path.Combine(_settings.WebRootPath, filePath);
        if (!System.IO.File.Exists(fullPath))
            return Task.CompletedTask;

        try
        {
            System.IO.File.Delete(fullPath);
            _logger.LogInformation("File deleted from disk: {FilePath}", filePath);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to delete file from disk: {FilePath}", filePath);
        }

        return Task.CompletedTask;
    }

    public async Task<FileContent?> GetFileAsync(string filePath)
    {
        if (string.IsNullOrWhiteSpace(filePath))
            return null;

        var fullPath = Path.Combine(_settings.WebRootPath, filePath);
        if (!System.IO.File.Exists(fullPath))
        {
            _logger.LogDebug("File not found on disk, will fall back to DB: {FilePath}", filePath);
            return null;
        }

        var data = await System.IO.File.ReadAllBytesAsync(fullPath);
        _logger.LogDebug("File served from disk: {FilePath}", filePath);
        return BuildFileContent(data, filePath);
    }

    public FileContent BuildFileContent(byte[] data, string filePath)
    {
        var extension = Path.GetExtension(filePath).ToLowerInvariant();
        var contentType = FileServiceHelpers.ContentTypes.TryGetValue(extension, out var type) ? type : "application/octet-stream";
        return new FileContent(data, contentType, Path.GetFileName(filePath));
    }
}
