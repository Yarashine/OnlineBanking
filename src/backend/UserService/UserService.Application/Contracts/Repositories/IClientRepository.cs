using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UserService.Domain.Entities;

namespace UserService.Application.Contracts.Repositories;

public interface IClientRepository
{
    Task<Client> GetByIdAsync(string id, CancellationToken cancellation = default);
    Task<Client> GetByUserIdAsync(int id, CancellationToken cancellation = default);
    Task<IEnumerable<Client>> GetAllAsync(CancellationToken cancellation = default);
    Task DeleteAsync(string id, CancellationToken cancellation = default);
    Task CreateAsync(Client client, CancellationToken cancellation = default);
    Task UpdateAsync(Client client, CancellationToken cancellation = default);
}