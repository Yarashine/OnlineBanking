using System.Threading;
using System.Threading.Tasks;
using UserService.Application.DTOs.Requests;

namespace UserService.Application.Contracts.UseCases.Clients;

public interface ICreateUseCase
{
    Task ExecuteAsync(CreateClientRequest request, int userId, CancellationToken cancellationToken = default);
}
