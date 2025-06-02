namespace NotificationService.Application.UseCases.Notifications;

using Microsoft.Extensions.Logging;
using NotificationService.Application.Contracts.Repositories;
using NotificationService.Application.Contracts.UseCases.Notifications;

public class GetUnreadCountUseCase(
    INotificationRepository notificationRepository,
    ILogger<GetUnreadCountUseCase> logger) : IGetUnreadCountUseCase
{
    public async Task<int> ExecuteAsync(int userId, CancellationToken cancellation)
    {
        logger.LogInformation("Getting unread notification count for UserId: {UserId}", userId);

        var count = await notificationRepository.GetUnreadCountAsync(userId, cancellation);

        logger.LogInformation("Unread notification count for UserId {UserId}: {Count}", userId, count);

        return count;
    }
}