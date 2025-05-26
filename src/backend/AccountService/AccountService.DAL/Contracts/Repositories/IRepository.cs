using AccountService.DAL.Entities;

namespace AccountService.DAL.Contracts.Repositories;

public interface IRepository<T> where T : class
{
    Task CreateAsync(T account, CancellationToken cancellationToken);
    Task<T> GetByIdAsync(Guid accountId, CancellationToken cancellationToken);
    void Update(Account account, CancellationToken cancellationToken);
}