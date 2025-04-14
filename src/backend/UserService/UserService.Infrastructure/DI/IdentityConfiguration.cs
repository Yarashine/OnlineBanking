using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UserService.Domain.Entities;
using UserService.Infrastructure.Data;

namespace UserService.Infrastructure.DI;
public static class IdentityConfiguration
{
    public static IServiceCollection AddCustomIdentity(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("MSSQL")));

        services.AddIdentity<User, IdentityRole<int>>(options =>
        {
            options.Password.RequireDigit = true;
            options.Password.RequiredLength = 8;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = true;
            options.Password.RequireLowercase = true;
        })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

        return services;
    }

    public static IApplicationBuilder UseMigrationForMSSQL(this IApplicationBuilder app)
    {
        using (var scope = app.ApplicationServices.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            try
            {
                if (dbContext.Database.CanConnect())
                {
                    var pendingMigrations = dbContext.Database.GetPendingMigrations();
                    if (pendingMigrations.Any())
                    {
                        dbContext.Database.Migrate();
                        Console.WriteLine("Pending migrations applied successfully.");
                    }
                    else
                    {
                        Console.WriteLine("No pending migrations to apply.");
                    }
                }
                else
                {
                    dbContext.Database.Migrate();
                    Console.WriteLine("Database created and migrations applied successfully.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error applying migrations: {ex.Message}");
                throw;
            }
        }

        return app;
    }
}
