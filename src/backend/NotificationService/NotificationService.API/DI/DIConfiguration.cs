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
using System.Threading.Tasks;
using FluentValidation.AspNetCore;
using FluentValidation;
using System.Reflection;
using NotificationService.API.Services;
using UserClient;
using NotificationService.Infrastructure.Contracts.Services;
using Hangfire;
using Hangfire.Redis.StackExchange;
using Serilog;
using Serilog.Exceptions;
using Serilog.Formatting.Json;
using Confluent.Kafka;
using Confluent.Kafka.Admin;

public static class DIConfiguration
{
    public static IServiceCollection AddEnvVariables(this IServiceCollection services, IConfiguration configuration)
    {
        configuration["ConnectionStrings:MSSQL"] = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");
        configuration["ConnectionStrings:Redis"] = Environment.GetEnvironmentVariable("REDIS_CONNECTION_STRING");
        configuration["Email:Username"] = Environment.GetEnvironmentVariable("EMAIL_USERNAME");
        configuration["Email:Password"] = Environment.GetEnvironmentVariable("EMAIL_PASSWORD");
        configuration["Kafka:BootstrapServers"] = Environment.GetEnvironmentVariable("KAFKA_BOOTSTRAP_SERVERS");
        configuration["Logstash:Uri"] = Environment.GetEnvironmentVariable("LOGSTASH_URI");
        return services;
    }
    public static IServiceCollection AddConfigs(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<SmtpSettings>(configuration.GetSection(SmtpSettings.SectionName));
        services.Configure<PaginationSettings>(configuration.GetSection(PaginationSettings.SectionName));
        return services;
    }

    public static IServiceCollection AddConnectionStrings(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("MSSQL")));
        services.AddSingleton<IConnectionMultiplexer>(_ =>
            ConnectionMultiplexer.Connect(configuration.GetConnectionString("Redis")));
        return services;
    }

    public static IServiceCollection ConfigureLogging(this IServiceCollection services, IConfiguration configuration)
    {
        Log.Logger = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .Enrich.WithExceptionDetails()
            .WriteTo.Console()
            .WriteTo.Http(
                requestUri: configuration["Logstash:Uri"],
                queueLimitBytes: null,
                textFormatter: new JsonFormatter())
            .CreateLogger();
        return services;
    }

    public static IServiceCollection AddHangfire(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHangfire(config => config
            .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
            .UseSimpleAssemblyNameTypeSerializer()
            .UseRecommendedSerializerSettings()
            .UseRedisStorage(ConnectionMultiplexer.Connect(configuration.GetConnectionString("Redis"))));
        services.AddHangfireServer();
        return services;
    }

    public static IServiceCollection AddHealthChecks(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHealthChecks().AddDbContextCheck<AppDbContext>();

        services.AddHealthChecks()
            .AddSqlServer(configuration.GetConnectionString("MSSQL")!);

        services.AddHealthChecks()
            .AddRedis(configuration.GetConnectionString("Redis")!);
        return services;
    }

    public static IServiceCollection AddKafka(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<KafkaOptions>(configuration.GetSection("Kafka"));
        services.AddHostedService<KafkaConsumerService>();
        services.AddSingleton<IProducer<Null, string>>(sp =>
        {
            var config = new ProducerConfig { BootstrapServers = configuration["Kafka:BootstrapServers"] };
            return new ProducerBuilder<Null, string>(config).Build();
        });

        return services;
    }

    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddGrpcClient<UserService.UserServiceClient>(o =>
        {
            o.Address = new Uri("https://userservice:8081");
        })
        .ConfigureChannel(channel =>
        {
            channel.HttpHandler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
            };
        });
        services.AddScoped<IGrpcNotificationService, GrpcNotificationService>();
        services.AddScoped<ISendEmailConfirmationUseCase, SendEmailConfirmationUseCase>();
        services.AddScoped<ISendResetPasswordEmailUseCase, SendResetPasswordEmailUseCase>();
        services.AddScoped<IVerifyConfirmationUseCase, VerifyConfirmationUseCase>();
        services.AddScoped<IVerifyResetPasswordUseCase, VerifyResetPasswordUseCase>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<INotificationRepository, NotificationRepository>();
        services.AddScoped<ICreateUseCase, CreateUseCase>();
        services.AddScoped<IGetUnreadCountUseCase, GetUnreadCountUseCase>();
        services.AddScoped<IGetAllUseCase, GetAllUseCase>();
        services.AddScoped<IGetUnreadUseCase, GetUnreadUseCase>();
        services.AddScoped<IGetAllCountUseCase, GetAllCountUseCase>();
        services.AddMapping();
        return services;
    }


    public static IServiceCollection AddValidation(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddFluentValidationAutoValidation();

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

    public static async Task<IApplicationBuilder> UseMigration(this IApplicationBuilder app, CancellationToken cancellation = default)
    {
        using (var scope = app.ApplicationServices.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            if (dbContext.Database.GetPendingMigrations().Any())
            {
                await dbContext.Database.MigrateAsync(cancellation);
            }
        }
        return app;
    }
}
