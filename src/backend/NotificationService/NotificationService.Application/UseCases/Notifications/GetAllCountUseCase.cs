namespace NotificationService.Application.UseCases.Notifications;

using Microsoft.Extensions.Logging;
using NotificationService.Application.Contracts.Repositories;
using NotificationService.Application.Contracts.UseCases.Notifications;

public class GetAllCountUseCase(
    INotificationRepository notificationRepository,
    ILogger<GetUnreadCountUseCase> logger) : IGetAllCountUseCase
{
    public async Task<int> ExecuteAsync(int userId, CancellationToken cancellation)
    {
        logger.LogInformation("Getting all notification count for UserId: {UserId}", userId);

        var count = await notificationRepository.GetAllCountAsync(userId, cancellation);

        logger.LogInformation("All notification count for UserId {UserId}: {Count}", userId, count);

        return count;
    }
}