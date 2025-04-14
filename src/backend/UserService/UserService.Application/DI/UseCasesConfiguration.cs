using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using UserService.Application.Contracts.UseCases.Authorization;
using UserService.Application.Contracts.UseCases.Clients;
using UserService.Application.UseCases.Authorization;
using UserService.Application.UseCases.Clients;

namespace UserService.Application.DI;

public static class UseCasesConfiguration
{
    public static IServiceCollection AddUseCases(this IServiceCollection services)
    {
        services.AddScoped<ISignUpUseCase, SignUpUseCase>()
                .AddScoped<ISignInUseCase, SignInUseCase>()
                .AddScoped<IRefreshAccessTokenUseCase, RefreshAccessTokenUseCase>()
                .AddScoped<ILogOutUseCase, LogOutUseCase>()
                .AddScoped<ILogOutAllUseCase, LogOutAllUseCase>();

        services.AddScoped<ICreateUseCase, CreateUseCase>()
                .AddScoped<IUpdateUseCase, UpdateUseCase>()
                .AddScoped<IGetAllUseCase, GetAllUseCase>()
                .AddScoped<IDeleteUseCase, DeleteUseCase>()
                .AddScoped<IGetByIdUseCase, GetByIdUseCase>()
                .AddScoped<IGetByUserIdUseCase, GetByUserIdUseCase>();

        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        return services;
    }
}
