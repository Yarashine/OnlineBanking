using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UserService.Application.Contracts.Repositories;
using UserService.Application.Contracts.Services;
using UserService.Infrastructure.Repositories;
using UserService.Infrastructure.RepositoryInterfaces;
using UserService.Infrastructure.Services;

namespace UserService.Infrastructure.DI;

public static class ServicesConfiguration
{
    public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<ITokenService, TokenService>();

        services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>()
                .AddScoped<IClientRepository, ClientRepository>();
        return services;
    }
}
