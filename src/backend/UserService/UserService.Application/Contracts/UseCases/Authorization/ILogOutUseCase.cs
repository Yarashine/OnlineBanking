using System.Threading;
using System.Threading.Tasks;

namespace UserService.Application.Contracts.UseCases.Authorization;

public interface ILogOutUseCase
{
    Task ExecuteAsync(string refresh, CancellationToken cancellation = default);
}
