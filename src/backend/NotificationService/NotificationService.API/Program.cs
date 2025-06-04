namespace NotificationService.API;

using NotificationService.API.DI;
using NotificationService.Application.Hubs;
using Serilog;

public class Program
{
    public async static Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddEndpointsApiExplorer();

        builder.Services.AddControllers();
        builder.Services.AddOpenApi();

        builder.Services.AddSignalR();

        builder.Configuration
            .AddJsonFile("appsettings.json")
            .AddEnvironmentVariables();

        builder.Services.AddCors(options =>
        {
            options.AddDefaultPolicy(builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            });
        });

        builder.Services.AddEnvVariables(builder.Configuration);

        builder.Services.AddConfigs(builder.Configuration);

        builder.Services.AddConnectionStrings(builder.Configuration);

        builder.Services.AddHangfire(builder.Configuration);

        builder.Services.ConfigureLogging(builder.Configuration);

        builder.Host.UseSerilog();

        builder.Services.AddKafka(builder.Configuration);

        builder.Services.AddHealthChecks();

        builder.Services.AddValidation();

        builder.Services.AddServices();

        builder.Services.AddSwagger();

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        app.UseHttpsRedirection();

        app.UseCors();

        app.MapHub<NotificationHub>("/notificationHub");


        app.UseAuthorization();

        app.UseSwaggerConfig();

        app.MapControllers();

        await app.UseMigration();

        app.MapGet("/ping", () => "pong");

        app.Run();
    }
}
