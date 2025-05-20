using AccountService.DAL.Contracts.Repositories;
using AccountService.DAL.Data;
using AccountService.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace AccountService.DAL.Repositories;

public class TransferRepository(AccountDbContext dbContext) : BaseRepository<Transfer>, ITransferRepository
{
    public DbSet<Account> accounts = dbContext.Accounts;
    public DbSet<Transfer> transfers = dbContext.Transfers;
    public async Task CreateTransferAsync(Transfer transfer, CancellationToken cancellationToken = default)
    {
        await transfers.AddAsync(transfer, cancellationToken);

        var sender = await accounts
            .FirstOrDefaultAsync(a => a.Id == transfer.SenderAccountId, cancellationToken);
        sender.Balance -= transfer.Amount;
        sender.UpdatedAt = DateTime.UtcNow;

        var reciever = await accounts
            .FirstOrDefaultAsync(a => a.Id == transfer.ReceiverAccountId, cancellationToken);
        reciever.Balance += transfer.Amount;
        reciever.UpdatedAt = DateTime.UtcNow;

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<IEnumerable<Transfer>> GetAllByIdAsync(Guid id, int pageNumber, int pageSize, CancellationToken cancellationToken = default)
    {
        var query = transfers
            .Where(a => a.SenderAccountId == id || a.ReceiverAccountId == id)
            .AsNoTracking();

        query = ApplyPagination(query, pageNumber, pageSize);

        return await query.ToListAsync(cancellationToken);
    }
}