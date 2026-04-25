using WebApp.Domain.Common.Errors;

namespace WebApp.Domain.Common.Results.Interfaces;

public interface IResult
{
    Error Error { get; }
    bool IsFailure { get; }
    bool IsSuccess { get; }
}
