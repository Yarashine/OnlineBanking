using AccountService.DAL.Contracts.Repositories;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;

namespace AccountService.DAL.Repositories;

public class UnitOfWork(IDbContext dbContext, IServiceProvider serviceProvider) : IUnitOfWork
{
    private readonly Lazy<IAccountRepository> accounts = new(() => serviceProvider.GetRequiredService<IAccountRepository>());
    private readonly Lazy<ITransferRepository> transfers = new(() => serviceProvider.GetRequiredService<ITransferRepository>());
    private IDbContextTransaction transaction;

    public IAccountRepository AccountRepository => accounts.Value;

    public ITransferRepository TransferRepository => transfers.Value;

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (transaction != null)
        {
            throw new InvalidOperationException("A transaction is already in progress.");
        }

        transaction = await dbContext.BeginTransactionAsync(cancellationToken);
    }

    public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (transaction == null)
        {
            throw new InvalidOperationException("No transaction was started.");
        }

        await transaction.CommitAsync(cancellationToken);
        transaction.Dispose();
        transaction = null;
    }

    public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (transaction == null)
        {
            throw new InvalidOperationException("No transaction was started.");
        }

        await transaction.RollbackAsync(cancellationToken);
        transaction.Dispose();
        transaction = null;
    }

    public void Dispose()
    {
        transaction?.Dispose();
        dbContext.Dispose();
    }
}