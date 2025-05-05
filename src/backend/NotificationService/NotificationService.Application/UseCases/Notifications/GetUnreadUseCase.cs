namespace NotificationService.Application.UseCases.Notifications;

using AutoMapper;
using Microsoft.Extensions.Options;
using NotificationService.Application.Contracts.Repositories;
using NotificationService.Application.Contracts.UseCases.Notifications;
using NotificationService.Application.DTOs.Responses;
using NotificationService.Domain.Configs;

public class GetUnreadUseCase(
    IOptions<PaginationSettings> paginationOptions,
    INotificationRepository notificationRepository,
    IMapper mapper) : IGetUnreadUseCase
{
    private readonly PaginationSettings paginationSettings = paginationOptions.Value;
    public async Task<List<NotificationResponse>> ExecuteAsync(int userId, int pageNumber, int pageSize, CancellationToken cancellation)
    {
        if (pageSize <= 0)
        {
            pageSize = paginationSettings.DefaultPageSize;
        }

        var notifications = await notificationRepository.GetUnreadAsync(userId, pageNumber, pageSize, cancellation);

        return mapper.Map<List<NotificationResponse>>(notifications);
    }
}