namespace NotificationService.Application.Contracts.UseCases.Notifications;

using NotificationService.Application.DTOs.Responses;

public interface IGetAllUseCase 
{
    Task<List<NotificationResponse>> ExecuteAsync(int userId, int pageNumber = 1, int pageSize = -1, CancellationToken cancellation = default);
}