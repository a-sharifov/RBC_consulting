using RBC_consulting.Domain.Common.Errors;
using RBC_consulting.Domain.Common.Results.Interfaces;

namespace RBC_consulting.Domain.Common.Results;

public class Result : IResult
{
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public Error Error { get; }

    protected Result(bool isSuccess, Error error)
    {
        if (isSuccess && error != Error.None || !isSuccess && error == Error.None)
            throw new ArgumentException("Invalid success/error combination.");

        IsSuccess = isSuccess;
        Error = error;
    }

    public static Result Success() => new(true, Error.None);

    public static Result<TObject> Success<TObject>(TObject value) => new(value, true, Error.None);

    public static Result<TObject> Create<TObject>(TObject value, Error error)
        where TObject : class
        => value is null ? Failure<TObject>(error) : Success(value);

    public static Result Failure(Error error) => new(false, error);

    public static Result Failure(string code, string message) =>
        Failure(new Error(code, message));

    public static Result<TObject> Failure<TObject>(Error error) => new(default!, false, error);

    public static Result<TObject> Failure<TObject>(string code, string message) =>
        Failure<TObject>(new Error(code, message));

    public static Result FirstFailureOrSuccess(params Result[] results)
    {
        foreach (var result in results)
            if (result.IsFailure) return result;

        return Success();
    }
}
