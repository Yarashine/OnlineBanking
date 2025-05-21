using System.Threading;
using System.Threading.Tasks;
using UserService.Application.DTOs.Responses;

namespace UserService.Application.Contracts.UseCases.Clients;

public interface IGetByUserIdUseCase
{
    Task<ClientResponse> ExecuteAsync(int userId, CancellationToken cancellationToken = default);
}
