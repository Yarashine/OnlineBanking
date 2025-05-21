namespace NotificationService.Infrastructure.Data;

using Microsoft.EntityFrameworkCore;
using NotificationService.Domain.Entities;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Notification> Notifications { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
    }
}