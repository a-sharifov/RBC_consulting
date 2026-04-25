using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using WebApp.Application.Common.Behaviors;

namespace WebApp.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly);
            cfg.AddOpenBehaviors([
                typeof(ValidationBehavior<,>),
                typeof(LoggingPipelineBehavior<,>)]);
        });

        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);

        return services;
    }
}
