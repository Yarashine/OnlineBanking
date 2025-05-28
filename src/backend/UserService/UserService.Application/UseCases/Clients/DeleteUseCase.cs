using System.Threading;
using System.Threading.Tasks;
using UserService.Application.Contracts.Repositories;
using UserService.Application.Contracts.UseCases.Clients;
using UserService.Domain.Entities;
using UserService.Domain.Exceptions;

namespace UserService.Application.UseCases.Clients;

public class DeleteUseCase(IClientRepository clientRepository) : IDeleteUseCase
{
    public async Task ExecuteAsync(string id, CancellationToken cancellationToken = default)
    {
        var clientById = await clientRepository.GetByIdAsync(id, cancellationToken) ?? throw new NotFoundException("Client with this id doesn't exist");
        await clientRepository.DeleteAsync(id, cancellationToken);
    }
}
