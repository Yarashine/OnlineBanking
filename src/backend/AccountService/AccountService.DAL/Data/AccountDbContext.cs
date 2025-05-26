using AccountService.DAL.Contracts.Repositories;
using AccountService.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;

namespace AccountService.DAL.Data;

public class AccountDbContext(DbContextOptions<AccountDbContext> options) : DbContext(options), IDbContext
{
    public DbSet<Account> Accounts { get; set; } 
    public DbSet<Transfer> Transfers { get; set; } 

    public override DbSet<T> Set<T>() where T : class => base.Set<T>();

    public override EntityEntry<T> Entry<T>(T entity) where T : class => base.Entry(entity);

    public async Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        return await Database.BeginTransactionAsync(cancellationToken);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken) => await base.SaveChangesAsync(cancellationToken);

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>()
            .Property(a => a.Balance)
            .HasPrecision(18, 2);

        modelBuilder.Entity<Transfer>()
            .Property(t => t.Amount)
            .HasPrecision(18, 2);
    }
}