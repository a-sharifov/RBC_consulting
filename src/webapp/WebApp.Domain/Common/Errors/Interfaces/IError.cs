namespace WebApp.Domain.Common.Errors.Interfaces;

public interface IError
{
    string Code { get; }
    string Message { get; }
}
