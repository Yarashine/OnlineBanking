using MongoDB.Driver;
using UserService.Application.Contracts.Repositories;
using UserService.Domain.Entities;

namespace UserService.Infrastructure.Repositories;

public class ClientRepository(IMongoDatabase mongo) : IClientRepository
{
    private readonly IMongoCollection<Client> clients = mongo.GetCollection<Client>("Clients");
    public async Task CreateAsync(Client client, CancellationToken cancellation)
    {
        await clients.InsertOneAsync(client, new InsertOneOptions(), cancellation);
    }

    public async Task DeleteAsync(string id, CancellationToken cancellation)
    {
        await clients.DeleteOneAsync(id, cancellation);
    }

    public async Task<IEnumerable<Client>> GetAllAsync(CancellationToken cancellation)
    {
        return await clients.Find(_ => true).ToListAsync(cancellation);
    }

    public async Task<Client> GetByIdAsync(string id, CancellationToken cancellation)
    {
        return await clients.Find(c => c.Id == id).FirstOrDefaultAsync(cancellation);
    }

    public async Task<Client> GetByUserIdAsync(int userId, CancellationToken cancellation)
    {
        return await clients.Find(c => c.UserId == userId).FirstOrDefaultAsync(cancellation);
    }

    public async Task UpdateAsync(Client client, CancellationToken cancellation)
    {
        await clients.ReplaceOneAsync(c => c.Id == client.Id, client);
    }
}
