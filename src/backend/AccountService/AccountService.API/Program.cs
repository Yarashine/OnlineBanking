
using AccountService.BLL.UseCases.Account.Queries.GetAllByUserId;
using AccountService.DAL.Contracts.Repositories;
using AccountService.DAL.Data;
using AccountService.DAL.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using AccountService.BLL.MappingProfilies;
using Microsoft.OpenApi.Models;
using System;
using System.Reflection;

namespace AccountService.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Account API",
                    Version = "v1"
                });
            });

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddDbContext<AccountDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("MSSQL")));
            builder.Services.AddAutoMapper(typeof(AccountProfile).Assembly);

            builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetAllAccountsByUserIdQuery).Assembly));

            builder.Services.AddScoped<IAccountRepository, AccountRepository>();


            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Account API v1");
                });
            }

            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AccountDbContext>();
                dbContext.Database.Migrate();
            }


            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
