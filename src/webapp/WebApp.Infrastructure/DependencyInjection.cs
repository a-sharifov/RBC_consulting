using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WebApp.Contracts.File;
using WebApp.Contracts.Pdf;
using WebApp.Domain.EmployeeAggregate.Repositories;
using WebApp.Infrastructure.File;
using WebApp.Infrastructure.Pdf;
using WebApp.Infrastructure.Persistense;
using WebApp.Infrastructure.Persistense.Repositories.Employees;

namespace WebApp.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<FileSettings>(configuration.GetSection(FileSettings.SectionName));
        services.Configure<DatabaseSettings>(configuration.GetSection(DatabaseSettings.SectionName));

        services.AddScoped<ICommandEmployeeRepository, CommandEmployeeRepository>();
        services.AddScoped<IQueryEmployeeRepository, QueryEmployeeRepository>();
        services.AddScoped<IFileService, FileService>();
        services.AddScoped<IPdfService, PdfService>();

        return services;
    }
}
