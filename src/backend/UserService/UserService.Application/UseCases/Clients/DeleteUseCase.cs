using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using UserService.Application.Contracts.Repositories;
using UserService.Application.Contracts.UseCases.Clients;
using UserService.Domain.Exceptions;

namespace UserService.Application.UseCases.Clients;

public class DeleteUseCase(
    IClientRepository clientRepository,
    ILogger<DeleteUseCase> logger) : IDeleteUseCase
{
    public async Task ExecuteAsync(string id, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Deleting client with id: {ClientId}", id);

        var clientById = await clientRepository.GetByIdAsync(id, cancellationToken);
        if (clientById == null)
        {
            logger.LogWarning("Delete failed. Client with id {ClientId} not found.", id);
            throw new NotFoundException("Client with this id doesn't exist");
        }

        await clientRepository.DeleteAsync(id, cancellationToken);

        logger.LogInformation("Client with id {ClientId} deleted successfully.", id);
    }
}
