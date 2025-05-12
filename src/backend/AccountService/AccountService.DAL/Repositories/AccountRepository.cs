using AccountService.DAL.Contracts.Repositories;
using AccountService.DAL.Data;
using AccountService.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace AccountService.DAL.Repositories;

public class AccountRepository(AccountDbContext dbContext) : IAccountRepository
{
    public DbSet<Account> accounts = dbContext.Accounts;
    public DbSet<Transfer> transfers = dbContext.Transfers;

    public async Task CreateTransferAsync(Transfer transfer, CancellationToken cancellationToken = default)
    {
        await transfers.AddAsync(transfer, cancellationToken);
        var sender = await accounts.FirstOrDefaultAsync(a => a.Id == transfer.SenderAccountId, cancellationToken);
        sender.Balance -= transfer.Amount;
        sender.UpdatedAt = DateTime.UtcNow;
        var reciever = await accounts.FirstOrDefaultAsync(a => a.Id == transfer.ReceiverAccountId, cancellationToken);
        reciever.Balance += transfer.Amount;
        reciever.UpdatedAt = DateTime.UtcNow;
        await dbContext.SaveChangesAsync(cancellationToken);
    }

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

    public async Task<IEnumerable<Account>> GetAllByUserIdAsync(int userId, CancellationToken cancellationToken)
    {
        return await accounts
            .Where(a => a.UserId == userId)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Transfer>> GetAllTransfersByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await transfers
            .Where(a => a.SenderAccountId == id || a.ReceiverAccountId == id)
            .ToListAsync(cancellationToken);            
    }

    public async Task<Account> GetByIdAsync(Guid accountId, CancellationToken cancellationToken)
    {
        return await accounts.FirstOrDefaultAsync(a => a.Id == accountId, cancellationToken);
    }

    public async Task UpdateAsync(Account account, CancellationToken cancellationToken)
    {
        account.UpdatedAt = DateTime.UtcNow;
        dbContext.Entry(account).State = EntityState.Modified;
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}