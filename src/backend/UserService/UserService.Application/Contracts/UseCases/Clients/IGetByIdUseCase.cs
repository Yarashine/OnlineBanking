using System.Threading;
using System.Threading.Tasks;
using UserService.Application.DTOs.Responses;

namespace UserService.Application.Contracts.UseCases.Clients;

public interface IGetByIdUseCase
{
    Task<ClientResponse> ExecuteAsync(string id, CancellationToken cancellationToken = default);
}
