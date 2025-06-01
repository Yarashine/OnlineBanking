using AccountService.API.DI;

namespace AccountService.API
{
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

            builder.Services.AddConnetionStrings(builder.Configuration);

            builder.Services.AddConfigs(builder.Configuration);

            builder.Services.AddSwagger();

            builder.Services.AddServices();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwaggerConfig();
            }

            await app.UseMigration();

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
