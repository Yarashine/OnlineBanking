namespace NotificationService.Application.UseCases.Notifications;

using AutoMapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NotificationService.Application.Contracts.Repositories;
using NotificationService.Application.Contracts.UseCases.Notifications;
using NotificationService.Application.DTOs.Responses;
using NotificationService.Domain.Configs;

public class GetUnreadUseCase(
    IOptions<PaginationSettings> paginationOptions,
    INotificationRepository notificationRepository,
    IMapper mapper,
    ILogger<GetUnreadUseCase> logger) : IGetUnreadUseCase
{
    private readonly PaginationSettings paginationSettings = paginationOptions.Value;
    public async Task<List<NotificationResponse>> ExecuteAsync(int userId, int pageNumber, int pageSize, CancellationToken cancellation)
    {
        if (pageSize <= 0)
        {
            logger.LogWarning("Invalid pageSize {PageSize} provided. Using default: {DefaultPageSize}", pageSize, paginationSettings.DefaultPageSize);
            pageSize = paginationSettings.DefaultPageSize;
        }

        logger.LogInformation("Fetching unread notifications for UserId: {UserId}, PageNumber: {PageNumber}, PageSize: {PageSize}", userId, pageNumber, pageSize);

        var notifications = await notificationRepository.GetUnreadAsync(userId, pageNumber, pageSize, cancellation);

        logger.LogInformation("Fetched {Count} unread notifications for UserId: {UserId}", notifications.Count, userId);

        return mapper.Map<List<NotificationResponse>>(notifications);
    }
}