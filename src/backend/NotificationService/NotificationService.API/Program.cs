namespace NotificationService.API;

using NotificationService.API.DI;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddEndpointsApiExplorer();

        builder.Services.AddControllers();
        builder.Services.AddOpenApi();

        builder.Services.AddConfigs(builder.Configuration);

        builder.Services.AddConnetionStrings(builder.Configuration);

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

        app.UseMigration();

        app.Run();
    }
}
