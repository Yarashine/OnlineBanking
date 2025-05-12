using AccountService.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace AccountService.DAL.Data;

public class AccountDbContext(DbContextOptions<AccountDbContext> options) : DbContext(options)
{
    public DbSet<Account> Accounts { get; set; } 
    public DbSet<Transfer> Transfers { get; set; } 

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