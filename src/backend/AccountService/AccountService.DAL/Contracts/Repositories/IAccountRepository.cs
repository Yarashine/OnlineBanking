using AccountService.DAL.Entities;

namespace AccountService.DAL.Contracts.Repositories;

public interface IAccountRepository
{
    Task CreateTransferAsync(Transfer transfer, CancellationToken cancellationToken = default);
    Task<IEnumerable<Transfer>> GetAllTransfersByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Account> GetByIdAsync(Guid accountId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Account>> GetAllByUserIdAsync(int userId, CancellationToken cancellationToken = default);
    Task CreateAsync(Account account, CancellationToken cancellationToken = default);
    Task UpdateAsync(Account account, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid accountId, CancellationToken cancellationToken = default);
}