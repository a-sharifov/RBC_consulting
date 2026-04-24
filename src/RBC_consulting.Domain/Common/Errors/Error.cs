using RBC_consulting.Domain.Common.Errors.Interfaces;
using RBC_consulting.Domain.Common.ValueObjects;

namespace RBC_consulting.Domain.Common.Errors;

public sealed class Error : ValueObject, IError
{
    public string Code { get; }
    public string Message { get; }
    public ErrorType Type { get; }

    public Error(string code, string message, ErrorType type = ErrorType.Failure)
    {
        Code = code;
        Message = message;
        Type = type;
    }

    public static Error None => new(string.Empty, string.Empty, ErrorType.Failure);
    public static Error NullValue => new("Error.NullValue", "The specified result value is null.", ErrorType.Failure);

    public static Error Failure(string code, string message) => new(code, message, ErrorType.Failure);
    public static Error Validation(string code, string message) => new(code, message, ErrorType.Validation);
    public static Error Problem(string code, string message) => new(code, message, ErrorType.Problem);
    public static Error NotFound(string code, string message) => new(code, message, ErrorType.NotFound);
    public static Error Conflict(string code, string message) => new(code, message, ErrorType.Conflict);
    public static Error Unauthorized(string code, string message) => new(code, message, ErrorType.Unauthorized);
    public static Error Forbidden(string code, string message) => new(code, message, ErrorType.Forbidden);

    public static implicit operator string(Error error) => error?.Code ?? string.Empty;

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Code;
        yield return Message;
        yield return Type;
    }
}
