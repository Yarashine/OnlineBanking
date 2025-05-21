using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using UserService.Infrastructure.Configurations;

namespace UserService.Infrastructure.DI;

public static class MongoConfiguration
{
    public static IServiceCollection AddMongo(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration["MongoSettings:ConnectionString"];
        var databaseName = configuration["MongoSettings:DB"];

        ClientMongoConfiguration.Configure();

        var mongoClient = new MongoClient(connectionString);
        var database = mongoClient.GetDatabase(databaseName);

        services.AddSingleton<IMongoDatabase>(database);

        return services;
    }
}
