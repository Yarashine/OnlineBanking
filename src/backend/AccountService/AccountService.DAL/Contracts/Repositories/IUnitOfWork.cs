namespace AccountService.DAL.Contracts.Repositories;

public interface IUnitOfWork : IDisposable
{
    IAccountRepository AccountRepository { get; }
    ITransferRepository TransferRepository { get; }
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
    Task BeginTransactionAsync(CancellationToken cancellationToken = default);
    Task CommitTransactionAsync(CancellationToken cancellationToken = default);
    Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
}