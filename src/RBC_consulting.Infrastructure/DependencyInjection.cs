using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RBC_consulting.Contracts.File;
using RBC_consulting.Contracts.Pdf;
using RBC_consulting.Domain.EmployeeAggregate.Repositories;
using RBC_consulting.Infrastructure.File;
using RBC_consulting.Infrastructure.Pdf;
using RBC_consulting.Infrastructure.Persistense;
using RBC_consulting.Infrastructure.Persistense.Repositories.Employees;

namespace RBC_consulting.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration, string webRootPath)
    {
        services.Configure<FileSettings>(opts =>
        {
            configuration.GetSection(FileSettings.SectionName).Bind(opts);
            opts.WebRootPath = webRootPath;
        });
        services.Configure<DatabaseSettings>(configuration.GetSection(DatabaseSettings.SectionName));

        services.AddScoped<ICommandEmployeeRepository, CommandEmployeeRepository>();
        services.AddScoped<IQueryEmployeeRepository, QueryEmployeeRepository>();
        services.AddScoped<IFileService, FileService>();
        services.AddScoped<IPdfService, PdfService>();

        return services;
    }
}
