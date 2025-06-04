namespace NotificationTests;

using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using NotificationService.Domain.Entities;
using NotificationService.Infrastructure.Data;
using NotificationService.Infrastructure.Repositories;

public class NotificationRepositoryTests
{
    private static AppDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        return new AppDbContext(options);
    }

    [Fact]
    public async Task AddAsync_ShouldAddNotification()
    {
        using var context = CreateContext();
        var repository = new NotificationRepository(context);
        var notification = new Notification
        {
            Id = Guid.NewGuid(),
            UserId = 1,
            Message = "Test",
            CreatedAt = DateTime.UtcNow
        };

        await repository.AddAsync(notification, CancellationToken.None);

        var result = await context.Notifications.FirstOrDefaultAsync(n => n.Id == notification.Id);
        Assert.NotNull(result);
        Assert.Equal("Test", result.Message);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnPagedNotificationsForUser()
    {
        using var context = CreateContext();
        var userId = 1;

        for (int i = 0; i < 10; i++)
        {
            await context.Notifications.AddAsync(new Notification
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                Message = $"Message {i}",
                CreatedAt = DateTime.UtcNow.AddMinutes(-i)
            });
        }

        await context.SaveChangesAsync();

        var repository = new NotificationRepository(context);
        var result = await repository.GetAllAsync(userId, pageNumber: 1, pageSize: 5, CancellationToken.None);

        Assert.Equal(5, result.Count);
        Assert.True(result[0].CreatedAt > result[1].CreatedAt);
    }

    [Fact]
    public async Task GetUnreadAsync_ShouldReturnUnreadNotificationsAndMarkAsRead()
    {
        var userId = 1;

        var unread = new List<Notification>();
        for (int i = 0; i < 3; i++)
        {
            unread.Add(new Notification
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                Message = $"Unread {i}",
                IsRead = false,
                CreatedAt = DateTime.UtcNow.AddMinutes(-i)
            });
        }

        var read = new Notification
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Message = $"Read",
            IsRead = true,
            CreatedAt = DateTime.UtcNow
        };

        var connection = new SqliteConnection("Filename=:memory:");
        connection.Open();

        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite(connection)
            .Options;

        using var context = new AppDbContext(options);
        context.Database.EnsureCreated();

        await context.Notifications.AddRangeAsync(unread);
        await context.Notifications.AddAsync(read);
        await context.SaveChangesAsync();

        var repository = new NotificationRepository(context);
        var result = await repository.GetUnreadAsync(userId, pageNumber: 1, pageSize: 10, CancellationToken.None);

        Assert.Equal(3, result.Count);
        Assert.All(result, n => Assert.False(n.IsRead));
        var dbItems = await context.Notifications
            .AsNoTracking()
            .Where(n => n.UserId == userId && n.Message.StartsWith("Unread"))
            .ToListAsync();
        Assert.All(dbItems, n => Assert.True(n.IsRead));
    }

    [Fact]
    public async Task GetUnreadCountAsync_ShouldReturnCorrectCount()
    {
        using var context = CreateContext();
        var userId = 1;

        await context.Notifications.AddAsync(new Notification
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Message = "Unread 1",
            IsRead = false
        });

        await context.Notifications.AddAsync(new Notification
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Message = "Read 1",
            IsRead = true
        });

        await context.SaveChangesAsync();

        var repository = new NotificationRepository(context);
        var count = await repository.GetUnreadCountAsync(userId, CancellationToken.None);

        Assert.Equal(1, count);
    }
}
