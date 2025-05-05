namespace NotificationService.API.DI;

using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using NotificationService.Application.Contracts.Repositories;
using NotificationService.Application.Contracts.Services;
using NotificationService.Application.Contracts.UseCases.Email;
using NotificationService.Application.Contracts.UseCases.Notifications;
using NotificationService.Application.UseCases.Email;
using NotificationService.Application.UseCases.Notifications;
using NotificationService.Domain.Configs;
using NotificationService.Infrastructure.Data;
using NotificationService.Infrastructure.Repositories;
using NotificationService.Infrastructure.Services;
using StackExchange.Redis;
using NotificationService.Application.DI;

public static class DIConfiguration
{
    public static IServiceCollection AddConfigs(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<SmtpSettings>(configuration.GetSection(SmtpSettings.SectionName));
        services.Configure<PaginationSettings>(configuration.GetSection(PaginationSettings.SectionName));
        return services;
    }

    public static IServiceCollection AddConnetionStrings(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("MSSQL")));
        services.AddSingleton<IConnectionMultiplexer>(_ =>
            ConnectionMultiplexer.Connect(configuration.GetConnectionString("Redis")));
        return services;
    }

    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<ISendEmailConfirmationUseCase, SendEmailConfirmationUseCase>();
        services.AddScoped<ISendResetPasswordEmailUseCase, SendResetPasswordEmailUseCase>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<INotificationRepository, NotificationRepository>();
        services.AddScoped<ICreateUseCase, CreateUseCase>();
        services.AddScoped<IGetUnreadCountUseCase, GetUnreadCountUseCase>();
        services.AddScoped<IGetAllUseCase, GetAllUseCase>();
        services.AddScoped<IGetUnreadUseCase, GetUnreadUseCase>();
        services.AddMapping();
        return services;
    }

    public static IServiceCollection AddSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Email API",
                Version = "v1"
            });
        });
        return services;
    }

    public static IApplicationBuilder UseSwaggerConfig(this IApplicationBuilder app)
    {
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "Email API v1");
        });
        return app;
    }

    public static IApplicationBuilder UseMigration(this IApplicationBuilder app)
    {
        using (var scope = app.ApplicationServices.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            dbContext.Database.Migrate();
        }
        return app;
    }
}
