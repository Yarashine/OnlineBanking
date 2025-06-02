using System.Text.Json;
using Confluent.Kafka;
using Microsoft.Extensions.Options;
using UserService.Application.Contracts.UseCases.Authorization;
using UserService.Domain.Configs;
using UserService.Domain.Exceptions;

namespace UserService.API.Services;

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

        var topics = new[] { kafkaOptions.Topics.ResetPass, kafkaOptions.Topics.ConfirmEmail };
        logger.LogInformation($"{topics}");
        consumer.Subscribe(topics);
        while (!cancellationToken.IsCancellationRequested)
        {
            var result = consumer.Consume(cancellationToken);
            if (result is not null && result.Topic == kafkaOptions.Topics.ConfirmEmail)
            {
                using var scope = scopeFactory.CreateScope();
                var confirmEmailUseCase = scope.ServiceProvider.GetRequiredService<IConfirmEmailUseCase>();

                var message = JsonSerializer.Deserialize<ConfirmMessage>(result.Message.Value) ?? throw new BadRequestException("Bad message for confirm token from kafka");
                logger.LogInformation("Before send email confirmation request Kafka in consumer in notification service");
                await confirmEmailUseCase.ExecuteAsync(message.Email, message.Token, cancellationToken);
                logger.LogInformation("After send email confirmation request Kafka in consumer in notification service");
            }
            else if (result is not null && result.Topic == kafkaOptions.Topics.ResetPass)
            {
                using var scope = scopeFactory.CreateScope();
                var resetPasswordUseCase = scope.ServiceProvider.GetRequiredService<IResetPasswordUseCase>();

                var message = JsonSerializer.Deserialize<ResetMessage>(result.Message.Value) ?? throw new BadRequestException("Bad message for reset token from kafka");
                logger.LogInformation("Before send email reset request Kafka in consumer in notification service");
                await resetPasswordUseCase.ExecuteAsync(message.Email, message.NewPassword, message.Token, cancellationToken);
                logger.LogInformation("After send email reset request Kafka in consumer in notification service");
            }
        }

        logger.LogInformation("After request Kafka in consumer in notification service");

        consumer.Close();
    }

    private record ConfirmMessage(string Email, string Token);
    private record ResetMessage(string Email, string NewPassword, string Token);
}