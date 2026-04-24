using RBC_consulting.Domain.Common.Errors;

namespace RBC_consulting.Domain.Common.Results.Interfaces;

public interface IValidationResult
{
    public static readonly Error ValidationError = new(
        "ValidationError",
        "A validation problem.");

    Error[] Errors { get; }
}
