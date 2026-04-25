using WebApp.Domain.Common.Errors;
using WebApp.Domain.Common.Results.Interfaces;

namespace WebApp.Domain.Common.Results;

public sealed class ValidationResult<TValue> : Result<TValue>, IValidationResult
{
    private ValidationResult(Error[] errors)
        : base(default!, false, IValidationResult.ValidationError) => Errors = errors;

    public Error[] Errors { get; }

    public static ValidationResult<TValue> WithErrors(params Error[] errors) => new(errors);
}
