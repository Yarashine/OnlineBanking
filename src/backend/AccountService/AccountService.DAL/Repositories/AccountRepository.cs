using AccountService.DAL.Contracts.Repositories;
using AccountService.DAL.Data;
using AccountService.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace AccountService.DAL.Repositories;

public class AccountRepository(AccountDbContext dbContext) : BaseRepository<Account>, IAccountRepository
{
    public DbSet<Account> accounts = dbContext.Accounts;
    public DbSet<Transfer> transfers = dbContext.Transfers;

    public async Task CreateAsync(Account account, CancellationToken cancellationToken)
    {
        await accounts.AddAsync(account, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Guid accountId, CancellationToken cancellationToken)
    {
        await accounts
            .Where(a => a.Id == accountId)
            .ExecuteDeleteAsync(cancellationToken);
    }

    public async Task<IEnumerable<Account>> GetAllByUserIdAsync(int userId, int pageNumber, int pageSize, CancellationToken cancellationToken)
    {
        var query = accounts
            .Where(a => a.UserId == userId)
            .AsNoTracking();

        query = ApplyPagination(query, pageNumber, pageSize);

        return await query.ToListAsync(cancellationToken);
    }

    public async Task<Account> GetByIdAsync(Guid accountId, CancellationToken cancellationToken)
    {
        return await accounts
            .AsNoTracking()
            .FirstOrDefaultAsync(a => a.Id == accountId, cancellationToken);
    }

    public async Task UpdateAsync(Account account, CancellationToken cancellationToken)
    {
        account.UpdatedAt = DateTime.UtcNow;
        dbContext.Entry(account).State = EntityState.Modified;
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}