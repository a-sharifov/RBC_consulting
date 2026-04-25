using WebApp.Domain.Common.Errors;
using WebApp.Domain.Common.Results.Interfaces;

namespace WebApp.Domain.Common.Results;

public sealed class ValidationResult : Result, IValidationResult
{
    private ValidationResult(Error[] errors)
        : base(false, IValidationResult.ValidationError) => Errors = errors;

    public Error[] Errors { get; }

    public static ValidationResult WithErrors(params Error[] errors) => new(errors);
}
