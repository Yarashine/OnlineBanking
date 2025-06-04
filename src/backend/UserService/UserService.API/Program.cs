using Serilog;
using UserService.API.DI;
using UserService.API.Middlewares;
using UserService.Application.DI;
using UserService.Infrastructure.DI;

namespace UserService.API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddControllers();
            builder.Services.AddOpenApi();
            builder.Configuration
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables();

            builder.Configuration["ConnectionStrings:MSSQL"] = Environment.GetEnvironmentVariable("MSSQL_CONNECTION")
               ?? throw new ArgumentException("MSSQL_CONNECTION environment variable is not set.");
            builder.Configuration["ConnectionStrings:Redis"] = Environment.GetEnvironmentVariable("REDIS_CONNECTION")
                ?? throw new ArgumentException("REDIS_CONNECTION environment variable is not set.");
            builder.Configuration["MongoSettings:ConnectionString"] = Environment.GetEnvironmentVariable("MONGO_CONNECTION")
                ?? throw new ArgumentException("MONGO_CONNECTION environment variable is not set.");
            builder.Configuration["Jwt:PrivateKeyPath"] = Environment.GetEnvironmentVariable("JWT_PRIVATE_KEY_PATH")
                ?? throw new ArgumentException("JWT_PRIVATE_KEY_PATH environment variable is not set.");
            builder.Configuration["Jwt:PublicKey"] = Environment.GetEnvironmentVariable("JWT_PUBLIC_KEY")
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

            builder.Services.AddHttpContextAccessor()
                .AddRedis(builder.Configuration)
                .AddMongo(builder.Configuration)
                .ConfigureJwt(builder.Configuration)
                .AddCustomIdentity(builder.Configuration)
                .AddSwagger()
                .AddJwt(builder.Configuration)
                .AddHealthChecks(builder.Configuration)
                .AddUseCases()
                .AddServices(builder.Configuration)
                .AddValidation()
                .AddKafka(builder.Configuration)
                .AddGrpcServices()
                .ConfigureLogging(builder.Configuration);

            builder.Host.UseSerilog();

            var app = builder.Build();

            app.UseMiddleware<ExceptionHandlingMiddleware>();
            app.UseCustomSwagger();

            await app.UseMigrationForMSSQL();

            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.AddRoles();
            app.UseGrpc();

            app.UseCors();

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseMapHealth();

            app.MapControllers();

            app.Run();
        }
    }
}
