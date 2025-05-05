namespace NotificationService.Application.Contracts.Repositories;

using NotificationService.Domain.Entities;

public interface INotificationRepository
{
    Task AddAsync(Notification notification, CancellationToken cancellation = default);
    Task<List<Notification>> GetAllAsync(int userId, int pageNumber, int pageSize, CancellationToken cancellation = default);
    Task<List<Notification>> GetUnreadAsync(int userId, int pageNumber, int pageSize, CancellationToken cancellation = default);
    Task<int> GetUnreadCountAsync(int userId, CancellationToken cancellation = default);
}