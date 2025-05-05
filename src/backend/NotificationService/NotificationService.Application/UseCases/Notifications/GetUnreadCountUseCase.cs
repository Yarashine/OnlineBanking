namespace NotificationService.Application.UseCases.Notifications;

using NotificationService.Application.Contracts.Repositories;
using NotificationService.Application.Contracts.UseCases.Notifications;

public class GetUnreadCountUseCase(INotificationRepository notificationRepository) : IGetUnreadCountUseCase
{
    public async Task<int> ExecuteAsync(int userId, CancellationToken cancellation)
    {
        return await notificationRepository.GetUnreadCountAsync(userId, cancellation);
    }
}