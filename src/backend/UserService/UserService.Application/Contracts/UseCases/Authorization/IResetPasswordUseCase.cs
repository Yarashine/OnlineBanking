using System.Threading;
using System.Threading.Tasks;

namespace UserService.Application.Contracts.UseCases.Authorization;

public interface IResetPasswordUseCase
{
    Task ExecuteAsync(string userId, string password, string token, CancellationToken cancellation = default);
}
