using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using UserService.Domain.Constants;

namespace UserService.Infrastructure.DI;

public static class RoleConfiguration
{
    public static IApplicationBuilder AddRoles(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<int>>>();

        var roles = Enum.GetNames<Role>();
        foreach (var role in roles)
        {
            var isRoleExist = roleManager.RoleExistsAsync(role).Result;
            if (!isRoleExist)
            {
                var r = roleManager.CreateAsync(new IdentityRole<int>(role)).Result;
            }
        }

        return app;
    }
}