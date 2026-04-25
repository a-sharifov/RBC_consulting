using WebApp.Domain.Common.Errors;

namespace WebApp.Domain.Common.Results.Interfaces;

public interface IValidationResult
{
    public static readonly Error ValidationError = new(
        "ValidationError",
        "A validation problem.");

    Error[] Errors { get; }
}
