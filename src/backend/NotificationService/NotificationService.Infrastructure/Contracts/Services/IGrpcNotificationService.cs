namespace NotificationService.Infrastructure.Contracts.Services;

public interface IGrpcNotificationService
{
    Task<string> GenerateConfirmationTokenAsync(string email, CancellationToken cancellation = default);
}