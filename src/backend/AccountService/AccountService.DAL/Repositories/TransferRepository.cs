using AccountService.DAL.Contracts.Repositories;
using AccountService.DAL.Data;
using AccountService.DAL.Entities;
using AccountService.DAL.Extensions;
using Microsoft.EntityFrameworkCore;

namespace AccountService.DAL.Repositories;

public class TransferRepository(IDbContext dbContext) : BaseRepository<Transfer>(dbContext), ITransferRepository
{
    private readonly DbSet<Transfer> transfers = dbContext.Transfers;

    public async Task<IEnumerable<Transfer>> GetAllByIdAsync(Guid id, int pageNumber, int pageSize, CancellationToken cancellationToken = default)
    {
        return await transfers
            .Where(a => a.SenderAccountId == id || a.ReceiverAccountId == id)
            .Paginate(pageNumber, pageSize)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }
}