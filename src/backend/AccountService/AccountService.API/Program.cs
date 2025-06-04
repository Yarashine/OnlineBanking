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

            builder.Configuration["ConnectionStrings:MSSQL"] = Environment.GetEnvironmentVariable("MSSQL_CONNECTION")
               ?? throw new ArgumentException("MSSQL_CONNECTION environment variable is not set.");
            builder.Configuration["Kafka:BootstrapServers"] = Environment.GetEnvironmentVariable("KAFKA_BOOTSTRAP")
                ?? throw new ArgumentException("KAFKA_BOOTSTRAP environment variable is not set.");
            builder.Configuration["Logstash:Uri"] = Environment.GetEnvironmentVariable("LOGSTASH_URI")
                ?? throw new ArgumentException("LOGSTASH_URI environment variable is not set.");

            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder.AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader();
                });
            });

            builder.Services.AddConnetionStrings(builder.Configuration);

            builder.Services.AddConfigs(builder.Configuration);

            builder.Services.AddSwagger();

            builder.Services.AddKafka(builder.Configuration);

            builder.Services.AddServices();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwaggerConfig();
            }

            await app.UseMigration();

            app.UseHttpsRedirection();

            app.UseCors();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
