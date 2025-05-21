namespace NotificationService.Application.UseCases.Notifications;

using AutoMapper;
using NotificationService.Application.Contracts.Repositories;
using NotificationService.Application.Contracts.UseCases.Notifications;
using NotificationService.Application.DTOs.Requests;
using NotificationService.Domain.Entities;

public class CreateUseCase(
    IMapper mapper, 
    INotificationRepository notificationRepository) : ICreateUseCase
{
    public async Task ExecuteAsync(CreateNotificationRequest request, CancellationToken cancellation)
    {
        var notification = mapper.Map<Notification>(request);

        await notificationRepository.AddAsync(notification, cancellation);
    }
}