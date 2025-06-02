namespace NotificationService.Application.UseCases.Notifications;

using AutoMapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NotificationService.Application.Contracts.Repositories;
using NotificationService.Application.Contracts.UseCases.Notifications;
using NotificationService.Application.DTOs.Responses;
using NotificationService.Domain.Configs;

public class GetAllUseCase(
    IOptions<PaginationSettings> paginationOptions,
    INotificationRepository notificationRepository,
    IMapper mapper,
    ILogger<GetAllUseCase> logger) : IGetAllUseCase
{
    private readonly PaginationSettings paginationSettings = paginationOptions.Value;
    public async Task<List<NotificationResponse>> ExecuteAsync(int userId, int pageNumber, int pageSize, CancellationToken cancellation)
    {

        logger.LogInformation("Fetching all notifications for UserId: {UserId}, PageNumber: {PageNumber}, PageSize: {PageSize}", userId, pageNumber, pageSize);


        if (pageSize <= 0)
        {
            pageSize = paginationSettings.DefaultPageSize;
            logger.LogWarning("Page size was invalid. Falling back to default: {DefaultPageSize}", pageSize);

        }

        var notifications = await notificationRepository.GetAllAsync(userId, pageNumber, pageSize, cancellation);


        logger.LogInformation("Fetched {Count} total notifications for UserId: {UserId}", notifications.Count, userId);


        return mapper.Map<List<NotificationResponse>>(notifications);
    }
}