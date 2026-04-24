using RBC_consulting.Domain.Common.Errors;

namespace RBC_consulting.Domain.Common.Results.Interfaces;

public interface IResult
{
    Error Error { get; }
    bool IsFailure { get; }
    bool IsSuccess { get; }
}
