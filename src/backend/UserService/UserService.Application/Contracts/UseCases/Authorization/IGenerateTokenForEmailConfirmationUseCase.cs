using System.Threading;
using System.Threading.Tasks;

namespace UserService.Application.Contracts.UseCases.Authorization;

public interface IGenerateTokenForEmailConfirmationUseCase
{
    Task ExecuteAsync(string userId, CancellationToken cancellation = default);
}
