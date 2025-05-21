namespace NotificationService.Application.Contracts.UseCases.Notifications;

using NotificationService.Application.DTOs.Responses;

public interface IGetUnreadUseCase
{
    Task<List<NotificationResponse>> ExecuteAsync(int userId, int pageNumber = 1, int pageSize = -1, CancellationToken cancellation = default);
}