using UserService.API.DI;
using UserService.API.Middlewares;
using UserService.Application.DI;
using UserService.Infrastructure.DI;

namespace UserService.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddControllers();
            builder.Services.AddOpenApi();

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
                .AddValidation();

            var app = builder.Build();

            app.UseMiddleware<ExceptionHandlingMiddleware>();
            app.UseCustomSwagger();
            app.UseMigrationForMSSQL();

            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.AddRoles();

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
