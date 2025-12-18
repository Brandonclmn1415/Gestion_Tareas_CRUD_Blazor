using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using SIG_T.Domain.Common.Options.Database;
using SIG_T.Persistence.Data;

namespace SIG_T.Persistence;

public static class DependecyInjection
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        //EF Core
        services.ConfigureOptions<DatabaseOptionsSetup>();

        services.AddDbContextFactory<ApplicationDbContext>(
            (serviceProvider, dbContextOptionsBuilder) =>
            {
                var databaseOptions = serviceProvider.GetService<Microsoft.Extensions.Options.IOptions<DatabaseOptions>>()!.Value;

                dbContextOptionsBuilder.UseSqlServer(databaseOptions.ConnectionString, sqlServerOptionsAction =>
                {
                    sqlServerOptionsAction.EnableRetryOnFailure(databaseOptions.MaxRetryCount);
                    sqlServerOptionsAction.CommandTimeout(databaseOptions.CommandTimeout);
                });
                dbContextOptionsBuilder.EnableDetailedErrors(databaseOptions.EnableDetailedErrors);
                dbContextOptionsBuilder.EnableSensitiveDataLogging(databaseOptions.EnableSensitiveDataLogging);
            });

        #region Perfilamiento
        //services.AddScoped<IUsuarioRepository, UsuarioRepository>();
        #endregion

        return services;
    }
}
