using UserService.API.Services.GRPC;
using UserService.Application.Contracts.UseCases.Authorization;
using UserService.Application.UseCases.Authorization;

namespace UserService.API.DI;

public static class GrpcConfiguration
{
    public static IServiceCollection AddGrpcServices(this IServiceCollection services)
    {
        services.AddGrpc();
        services.AddScoped<UserGrpcService>();
        services.AddScoped<IGenerateTokenForEmailConfirmationUseCase, GenerateTokenForEmailConfirmationUseCase>();
        services.AddScoped<IGenerateTokenForResetPassword, GenerateTokenForResetPassword>();
        return services;
    }

    public static IEndpointRouteBuilder UseGrpc(this IEndpointRouteBuilder app)
    {
        app.MapGrpcService<UserGrpcService>();
        return app;
    }
}
