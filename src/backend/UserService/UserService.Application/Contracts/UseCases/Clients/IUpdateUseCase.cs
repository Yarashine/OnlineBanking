using System.Threading;
using System.Threading.Tasks;
using UserService.Application.DTOs.Requests;

namespace UserService.Application.Contracts.UseCases.Clients;

public interface IUpdateUseCase
{
    Task ExecuteAsync(UpdateClientRequest request, CancellationToken cancellationToken = default);
}
