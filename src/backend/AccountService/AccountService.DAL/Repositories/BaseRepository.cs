using AccountService.DAL.Contracts.Repositories;
using AccountService.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace AccountService.DAL.Repositories;

public class BaseRepository<T>(IDbContext dbContext) : IRepository<T> where T : Entity
{
    private readonly DbSet<T> dbSet = dbContext.Set<T>();

    public async Task CreateAsync(T account, CancellationToken cancellationToken)
    {
        await dbSet.AddAsync(account, cancellationToken);
    }

    public void Update(Account account, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        account.UpdatedAt = DateTime.UtcNow;
        dbContext.Entry(account).State = EntityState.Modified;
    }

    public async Task<T> GetByIdAsync(Guid accountId, CancellationToken cancellationToken)
    {
        return await dbSet
            .AsNoTracking()
            .FirstOrDefaultAsync(a => a.Id == accountId, cancellationToken);
    }
}