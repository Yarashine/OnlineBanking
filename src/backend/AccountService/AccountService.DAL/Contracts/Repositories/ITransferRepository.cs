﻿using AccountService.DAL.Entities;

namespace AccountService.DAL.Contracts.Repositories;

public interface ITransferRepository
{
    Task CreateAsync(Transfer transfer, CancellationToken cancellationToken = default);
    Task<IEnumerable<Transfer>> GetAllByIdAsync(Guid id, int pageNumber, int pageSize, CancellationToken cancellationToken = default);
}