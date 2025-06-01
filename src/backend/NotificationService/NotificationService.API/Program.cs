namespace NotificationService.API;

using NotificationService.API.DI;
using Serilog;

public class Program
{
    public async static Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddEndpointsApiExplorer();

        builder.Services.AddControllers();
        builder.Services.AddOpenApi();

        builder.Configuration
            .AddJsonFile("appsettings.json")
            .AddEnvironmentVariables();

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

        app.UseAuthorization();

        app.UseSwaggerConfig();

        app.MapControllers();

        await app.UseMigration();

        app.MapGet("/ping", () => "pong");

        app.Run();
    }
}
