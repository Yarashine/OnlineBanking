using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NotificationService.Application.Contracts.Services;
using NotificationService.Domain.Configs;
using System.Text.Json;

namespace NotificationService.Application.UseCases.Email;

public class VerifyResetPasswordUseCase(
    ITokenService tokenService,
    IOptions<KafkaOptions> options,
    IProducer<Null, string> kafkaProducer,
    ILogger<VerifyConfirmationUseCase> logger) : IVerifyResetPasswordUseCase
{
    private readonly KafkaOptions options = options.Value;

    public async Task ExecuteAsync(string token, CancellationToken cancellation)
    {
        logger.LogInformation("Verifying reset token");

        var resetData = await tokenService.VerifyResetTokenAsync(token);

        logger.LogInformation("Reset token verified for email: {Email}", resetData.Email);
        logger.LogInformation("Publishing reset password message to Kafka topic: {Topic}", options.Topics.ResetPass);

        var resetMessage = new ResetMessage(resetData.Email, resetData.NewPassword, token);
        var serialisedMessage = JsonSerializer.Serialize(resetMessage);

        await kafkaProducer.ProduceAsync(options.Topics.ResetPass, new Message<Null, string> { Value = serialisedMessage }, cancellation);

        logger.LogInformation("Reset password message published to Kafka for email: {Email}", resetData.Email);
    }

    private record ResetMessage(string Email, string NewPassword, string Token);
}