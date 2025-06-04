using AccountService.DAL.Contracts.Repositories;
using AccountService.DAL.Data;
using AccountService.DAL.Entities;
using AccountService.DAL.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace AccountService.DAL.Repositories;

public class AccountRepository(IDbContext dbContext) : BaseRepository<Account>(dbContext), IAccountRepository
{
    public DbSet<Account> accounts = dbContext.Accounts;

    public async Task DeleteAsync(Guid accountId, CancellationToken cancellationToken)
    {
        await accounts
            .Where(a => a.Id == accountId)
            .ExecuteDeleteAsync(cancellationToken);
    }

    public async Task<IEnumerable<Account>> GetAllByUserIdAsync(int userId, int pageNumber, int pageSize, CancellationToken cancellationToken)
    {
        return await accounts
            .Where(a => a.UserId == userId)
            .Paginate(pageNumber, pageSize)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Account>> GetAllAsync(int pageNumber, int pageSize, CancellationToken cancellationToken)
    {
        return await accounts
            .Paginate(pageNumber, pageSize)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }
}