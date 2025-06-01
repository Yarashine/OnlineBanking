using Confluent.Kafka;
using Confluent.Kafka.Admin;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using NotificationService.Application.Contracts.UseCases.Email;
using NotificationService.Application.Contracts.UseCases.Notifications;
using NotificationService.Application.DTOs.Requests;
using NotificationService.Domain.Configs;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace NotificationService.API.Services;

public class KafkaConsumerService : BackgroundService
{
    private readonly KafkaOptions kafkaOptions;
    private readonly IConsumer<Null, string> consumer;
    private readonly IServiceScopeFactory scopeFactory;
    private readonly ILogger logger;

    public KafkaConsumerService(IOptions<KafkaOptions> options, IServiceScopeFactory _scopeFactory, ILogger<KafkaConsumerService> _logger)
    {
        scopeFactory = _scopeFactory;
        logger = _logger;
        kafkaOptions = options.Value;

        var config = new ConsumerConfig
        {
            BootstrapServers = kafkaOptions.BootstrapServers,
            GroupId = "notification-service",
            AutoOffsetReset = AutoOffsetReset.Earliest
        };

        logger.LogInformation($"Kafka constructor notification service {kafkaOptions.BootstrapServers} {kafkaOptions.Topics.EmailConfirmation} {kafkaOptions.Topics.ConfirmEmail}");

        consumer = new ConsumerBuilder<Null, string>(config).Build();
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        await Task.Yield();
        logger.LogInformation("Before request Kafka in consumer in notification service");

        var topics = new[] { kafkaOptions.Topics.EmailConfirmation, kafkaOptions.Topics.SendNotification };
        consumer.Subscribe(topics);
        while (!cancellationToken.IsCancellationRequested)
        {
            var result = consumer.Consume(TimeSpan.FromSeconds(1));
            if (result != null && result.Topic == kafkaOptions.Topics.EmailConfirmation)
            {
                using var scope = scopeFactory.CreateScope();
                var sendEmailConfirmationUseCase = scope.ServiceProvider.GetRequiredService<ISendEmailConfirmationUseCase>();

                var message = JsonSerializer.Deserialize<EmailConfirmationMessage>(result.Message.Value);
                logger.LogInformation("Before send email confirmation request Kafka in consumer in notification service");
                await sendEmailConfirmationUseCase.ExecuteAsync(message.Email, cancellationToken);
                logger.LogInformation("After send email confirmation request Kafka in consumer in notification service");
            }
            else if (result != null && result.Topic == kafkaOptions.Topics.SendNotification)
            {
                using var scope = scopeFactory.CreateScope();
                var createUseCase = scope.ServiceProvider.GetRequiredService<ICreateUseCase>();

                var message = JsonSerializer.Deserialize<CreateNotificationRequest>(result.Message.Value);
                logger.LogInformation("Before create notification request Kafka in consumer in notification service");
                await createUseCase.ExecuteAsync(message, cancellationToken);
                logger.LogInformation("After create notification request Kafka in consumer in notification service");
            }
        }


        logger.LogInformation("After request Kafka in consumer in notification service");

        consumer.Close();
    }

    private record EmailConfirmationMessage(string Email);
}