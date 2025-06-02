using System.Threading;
using System.Threading.Tasks;

namespace UserService.Application.Contracts.UseCases.Authorization;

public interface IGenerateTokenForResetPassword
{
    Task<string> ExecuteAsync(string email, CancellationToken cancellation = default);
}
