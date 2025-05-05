namespace NotificationService.Application.Contracts.UseCases.Notifications;

using NotificationService.Application.DTOs.Requests;

public interface ICreateUseCase
{
    Task ExecuteAsync(CreateNotificationRequest request, CancellationToken cancellation = default);
}