using MediatR;
using Microsoft.Extensions.Logging;
using RBC_consulting.Domain.Common.Results;

namespace RBC_consulting.Application.Common.Behaviors;

public sealed class LoggingPipelineBehavior<TRequest, TResponse>
    (ILogger<LoggingPipelineBehavior<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : Result
{
    private readonly ILogger<LoggingPipelineBehavior<TRequest, TResponse>> _logger = logger;

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Handling {@RequestName}, {@DateTimeUtcNow}",
            typeof(TRequest).Name,
            DateTime.UtcNow);

        var response = await next(cancellationToken);

        if (response.IsSuccess)
        {
            _logger.LogInformation(
                "Handled {@RequestName}, {@DateTimeUtcNow}",
                typeof(TRequest).Name,
                DateTime.UtcNow);

            return response;
        }

        _logger.LogError(
            message: "Handled {@RequestName}, {@Error} {@DateTimeUtcNow} error",
            typeof(TRequest).Name,
            response.Error,
            DateTime.UtcNow);

        return response;
    }
}