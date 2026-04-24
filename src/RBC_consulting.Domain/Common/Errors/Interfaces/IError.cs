namespace RBC_consulting.Domain.Common.Errors.Interfaces;

public interface IError
{
    string Code { get; }
    string Message { get; }
}
