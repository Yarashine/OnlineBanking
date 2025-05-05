namespace NotificationService.Infrastructure.Repositories;

using Microsoft.EntityFrameworkCore;
using NotificationService.Application.Contracts.Repositories;
using NotificationService.Domain.Entities;
using NotificationService.Infrastructure.Data;

public class NotificationRepository(AppDbContext context) : INotificationRepository
{
    private readonly DbSet<Notification> notifications = context.Notifications;

    public async Task AddAsync(Notification notification, CancellationToken cancellation)
    {
        await this.notifications.AddAsync(notification, cancellation);
        await context.SaveChangesAsync(cancellation);
    }

    public async Task<List<Notification>> GetAllAsync(int userId, int pageNumber, int pageSize, CancellationToken cancellation)
    {
        return await this.notifications
            .Where(n => n.UserId == userId)
            .OrderByDescending(n => n.CreatedAt)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellation);
    }

    public async Task<List<Notification>> GetUnreadAsync(int userId, int pageNumber, int pageSize, CancellationToken cancellation)
    {
        var nots = await this.notifications
            .AsNoTracking()
            .Where(n => n.UserId == userId && !n.IsRead)
            .OrderByDescending(n => n.CreatedAt)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellation);

        await this.notifications
            .Where(n => n.UserId == userId && !n.IsRead)
            .OrderByDescending(n => n.CreatedAt)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ExecuteUpdateAsync(n => n.SetProperty(n => n.IsRead, true), cancellation);

        return nots;
    }

    public async Task<int> GetUnreadCountAsync(int userId, CancellationToken cancellation)
    {
        return await this.notifications
            .CountAsync(n => n.UserId == userId && n.IsRead == false, cancellation);
    }
}