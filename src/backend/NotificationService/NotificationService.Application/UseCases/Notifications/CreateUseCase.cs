namespace NotificationService.Application.UseCases.Notifications;

using AutoMapper;
using Microsoft.Extensions.Logging;
using NotificationService.Application.Contracts.Repositories;
using NotificationService.Application.Contracts.UseCases.Notifications;
using NotificationService.Application.DTOs.Requests;
using NotificationService.Domain.Entities;

public class CreateUseCase(
    IMapper mapper, 
    INotificationRepository notificationRepository,
    ILogger<CreateUseCase> logger) : ICreateUseCase
{
    public async Task ExecuteAsync(CreateNotificationRequest request, CancellationToken cancellation)
    {
        logger.LogInformation("Creating a new notification for UserId: {UserId}", request.UserId);

        var notification = mapper.Map<Notification>(request);

        await notificationRepository.AddAsync(notification, cancellation);

        logger.LogInformation("Notification created successfully for UserId: {UserId}", request.UserId);
    }
}