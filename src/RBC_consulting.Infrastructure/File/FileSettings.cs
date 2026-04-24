namespace RBC_consulting.Infrastructure.File;

public sealed class FileSettings
{
    public const string SectionName = "FileSettings";
    public string UploadFolderName { get; set; } = "uploads";
    public string[] AllowedExtensions { get; set; } = [ ".pdf", ".jpg", ".jpeg", ".png" ];
    public string WebRootPath { get; set; } = string.Empty;
}
