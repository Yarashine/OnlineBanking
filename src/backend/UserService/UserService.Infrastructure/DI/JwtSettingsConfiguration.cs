using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UserService.Infrastructure.Configs;

namespace UserService.Infrastructure.DI;

public static class JwtSettingsConfiguration
{
    public static IServiceCollection ConfigureJwt(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<JwtOptions>(configuration.GetSection("JwtSettings"));
        return services;
    }
}