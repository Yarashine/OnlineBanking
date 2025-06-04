using AccountService.DAL.Entities;

namespace AccountService.DAL.Contracts.Repositories;

public interface IAccountRepository
{
    Task<Account> GetByIdAsync(Guid accountId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Account>> GetAllByUserIdAsync(int userId, int pageNumber, int pageSize, CancellationToken cancellationToken = default);
    Task<IEnumerable<Account>> GetAllAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default);
    Task CreateAsync(Account account, CancellationToken cancellationToken = default);
    void Update(Account account, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid accountId, CancellationToken cancellationToken = default);
}