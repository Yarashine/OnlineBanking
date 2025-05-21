using System.Threading;
using System.Threading.Tasks;
using UserService.Application.Contracts.Repositories;
using UserService.Application.Contracts.UseCases.Clients;

namespace UserService.Application.UseCases.Clients;

public class DeleteUseCase(IClientRepository clientRepository) : IDeleteUseCase
{
    public async Task ExecuteAsync(string id, CancellationToken cancellationToken = default)
    {
        await clientRepository.DeleteAsync(id, cancellationToken);
    }
}
