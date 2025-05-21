using System.Threading;
using System.Threading.Tasks;

namespace UserService.Application.Contracts.UseCases.Clients;

public interface IDeleteUseCase
{
    Task ExecuteAsync(string id, CancellationToken cancellationToken = default);
}
