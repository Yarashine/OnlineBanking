namespace NotificationService.Application.DI;

using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

public static class MappingConfiguration
{
    public static IServiceCollection AddMapping(this IServiceCollection services)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());

        return services;
    }
}