using WebApp.Domain.Common.Errors;

namespace WebApp.Domain.Common.Results;

public class Result<TObject> : Result
{
    private readonly TObject _value;

    protected internal Result(TObject value, bool isSuccess, Error error)
        : base(isSuccess, error) => _value = value;

    public TObject Value => IsSuccess
        ? _value
        : throw new InvalidOperationException("The value of a failure result cannot be accessed.");

    public static implicit operator Result<TObject>(TObject value) => Success(value);
}
