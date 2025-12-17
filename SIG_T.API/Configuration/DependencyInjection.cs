using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SIG_T.API.Services.Interfaces;
using SIG_T.API.Services.Implementations;

namespace SIG_T.API.Configuration;

/// <summary>
/// Dependency Injection Configuration
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Register application services
    /// </summary>
    /// <param name="services">Service collection</param>
    /// <param name="configuration">Configuration</param>
    /// <returns>Service collection</returns>
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Register task services
        services.AddScoped<ITaskService, TaskService>();
        
        // Register activity logging service
        services.AddScoped<IActividadService, ActividadService>();

        // Register Tarea service (uses stored procedures for writes)
        services.AddScoped<ITareaService, TareaService>();

        return services;
    }
}
