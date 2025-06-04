namespace NotificationService.Application.UseCases.Notifications;

using AutoMapper;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using NotificationService.Application.Contracts.Repositories;
using NotificationService.Application.Contracts.UseCases.Notifications;
using NotificationService.Application.DTOs.Requests;
using NotificationService.Application.Hubs;
using NotificationService.Domain.Entities;

public class CreateUseCase(
    IMapper mapper, 
    INotificationRepository notificationRepository,
    ILogger<CreateUseCase> logger,
    IHubContext<NotificationHub> hubContext) : ICreateUseCase
{
    public async Task ExecuteAsync(CreateNotificationRequest request, CancellationToken cancellation)
    {
        logger.LogInformation("Creating a new notification for UserId: {UserId}", request.UserId);

        var notification = mapper.Map<Notification>(request);

        await notificationRepository.AddAsync(notification, cancellation);

        await hubContext.Clients.User(request.UserId.ToString())
            .SendAsync("ReceiveNotification", "You have new notification");

        logger.LogInformation("Notification created successfully for UserId: {UserId}", request.UserId);
    }
}