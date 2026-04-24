using FluentValidation;

namespace RBC_consulting.Application.Employees.Commands.UploadFile;

public sealed class UploadEmployeeFileCommandValidator : AbstractValidator<UploadEmployeeFileCommand>
{
    private static readonly string[] AllowedExtensions = [".pdf", ".jpg", ".jpeg", ".png"];
    private const long MaxFileSize = 5 * 1024 * 1024;

    public UploadEmployeeFileCommandValidator()
    {
        RuleFor(x => x.EmployeeId).GreaterThan(0);

        RuleFor(x => x.FileName)
            .NotEmpty()
            .MaximumLength(255)
            .Must(name => AllowedExtensions.Contains(Path.GetExtension(name).ToLower()))
            .WithMessage($"Allowed extensions: {string.Join(", ", AllowedExtensions)}");

        RuleFor(x => x.FileStream)
            .NotNull();

        RuleFor(x => x.FileSize)
            .GreaterThan(0)
            .LessThanOrEqualTo(MaxFileSize)
            .WithMessage($"File must be between 1 byte and {MaxFileSize / (1024 * 1024)} MB");
    }
}
