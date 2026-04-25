using FluentValidation;
using MediatR;
using WebApp.Domain.Common.Errors;
using WebApp.Domain.Common.Results.Interfaces;

namespace WebApp.Application.Common.Behaviors;

public sealed class ValidationBehavior<TRequest, TResponse>(
    IEnumerable<IValidator<TRequest>> validators)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : IResult
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (!validators.Any())
            return await next();

        var context = new ValidationContext<TRequest>(request);

        var failures = await Task.WhenAll(
            validators.Select(v => v.ValidateAsync(context, cancellationToken)));

        var messages = failures
            .Where(f => !f.IsValid)
            .SelectMany(f => f.Errors)
            .Select(e => e.ErrorMessage)
            .ToList();

        if (messages.Count == 0)
            return await next();

        var error = Error.Validation("Validation.Failed", string.Join(", ", messages));

        return (TResponse)typeof(TResponse)
            .GetMethod(nameof(Domain.Common.Results.Result.Failure), [typeof(Error)])!
            .Invoke(null, [error])!;
    }
}
