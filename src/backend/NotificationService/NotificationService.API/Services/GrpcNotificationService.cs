namespace NotificationService.API.Services;

using NotificationService.Infrastructure.Contracts.Services;
using UserClient;

public class GrpcNotificationService(
    UserService.UserServiceClient userServiceClient,
    ILogger<GrpcNotificationService> logger) : IGrpcNotificationService
{
    public async Task<string> GenerateConfirmationTokenAsync(string email, CancellationToken cancellation = default)
    {
        cancellation.ThrowIfCancellationRequested();

        logger.LogInformation("Before confirm request using Grpc in notification service");

        var response = await userServiceClient.GenerateEmailConfirmationTokenAsync(
            new GenerateTokenRequest { Email = email },
            cancellationToken: cancellation
        );

        logger.LogInformation($"After confirm request using Grpc in notification service {response}");

        return response.Token;
    }
        
    public async Task<string> GeneratePasswordResetToken(string email, CancellationToken cancellation = default)
    {
        cancellation.ThrowIfCancellationRequested();

        logger.LogInformation("Before reset request using Grpc in notification service");

        var response = await userServiceClient.GenerateEmailConfirmationTokenAsync(
            new GenerateTokenRequest { Email = email },
            cancellationToken: cancellation
        );

        logger.LogInformation($"After reset request using Grpc in notification service {response}");

        return response.Token;
    }
}
