using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using UserService.Infrastructure.Data;

namespace UserService.Infrastructure.DI;

public static class HealthCheckConfiguration
{
    public static IServiceCollection AddHealthChecks(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHealthChecks().AddDbContextCheck<ApplicationDbContext>();

        services.AddHealthChecks()
            .AddMongoDb(_ => new MongoClient(configuration["MongoSettings:ConnectionString"]));

        services.AddHealthChecks()
            .AddSqlServer(configuration.GetConnectionString("MSSQL")!);

        services.AddHealthChecks()
        .AddRedis(configuration.GetConnectionString("Redis")!);
        return services;
    }

    public static IApplicationBuilder UseMapHealth(this IApplicationBuilder app)
    {
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapHealthChecks("/health");
        });
        return app;
    }
}
