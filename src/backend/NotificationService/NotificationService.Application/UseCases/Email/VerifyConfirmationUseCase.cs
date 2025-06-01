using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NotificationService.Application.Contracts.Services;
using NotificationService.Application.Contracts.UseCases.Email;
using NotificationService.Domain.Configs;
using Confluent.Kafka;
using System.Text.Json;
namespace NotificationService.Application.UseCases.Email;

public class VerifyConfirmationUseCase(
    ITokenService tokenService,
    IOptions<KafkaOptions> options,
    IProducer<Null, string> kafkaProducer,
    ILogger<VerifyConfirmationUseCase> logger) : IVerifyConfirmationUseCase
{
    private readonly KafkaOptions options = options.Value;

    public async Task ExecuteAsync(string token, CancellationToken cancellation)
    {
        logger.LogInformation("Verifying email confirmation token");

        var email = await tokenService.VerifyConfirmationTokenAsync(token);

        logger.LogInformation("Email confirmation token verified for email: {Email}", email);
        logger.LogInformation("Publishing confirmation message to Kafka topic: {Topic}", options.Topics.ConfirmEmail);

        var confrimMessage = new ConfirmMessage(email, token);
        var serialisedMessage = JsonSerializer.Serialize(confrimMessage);

        await kafkaProducer.ProduceAsync(options.Topics.ConfirmEmail, new Message<Null, string> { Value = serialisedMessage }, cancellation);

        logger.LogInformation("Confirmation message published to Kafka for email: {Email}", email);
    }

    private record ConfirmMessage(string Email, string Token);
}