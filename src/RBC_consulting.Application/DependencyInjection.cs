using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using RBC_consulting.Application.Common.Behaviors;

namespace RBC_consulting.Application;

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
