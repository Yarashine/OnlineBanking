using System.Reflection;
using FluentValidation;
using FluentValidation.AspNetCore;
using UserService.API.Validators;

namespace UserService.API.DI;

public static class ValidationConfiguration
{
    public static IServiceCollection AddValidation(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddFluentValidationAutoValidation();

        return services;
    }
}
