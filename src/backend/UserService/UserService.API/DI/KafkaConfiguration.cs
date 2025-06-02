using Confluent.Kafka;
using Confluent.Kafka.Admin;
using UserService.API.Services;
using UserService.Application.Contracts.UseCases.Authorization;
using UserService.Application.UseCases.Authorization;
using UserService.Domain.Configs;

namespace UserService.API.DI;

public static class KafkaConfiguration
{
    public static IServiceCollection AddKafka(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<KafkaOptions>(configuration.GetSection("Kafka"));
        services.AddHostedService<KafkaConsumerService>();
        services.AddSingleton<IProducer<Null, string>>(sp =>
        {
            var config = new ProducerConfig { BootstrapServers = configuration["Kafka:BootstrapServers"] };
            return new ProducerBuilder<Null, string>(config).Build();
        });

        return services;
    }
}