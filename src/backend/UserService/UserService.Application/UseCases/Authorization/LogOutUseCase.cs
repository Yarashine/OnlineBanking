using System.Threading;
using System.Threading.Tasks;
using UserService.Application.Contracts.Services;
using UserService.Application.Contracts.UseCases.Authorization;

namespace UserService.Application.UseCases.Authorization;

public class LogOutUseCase(ITokenService tokenService) : ILogOutUseCase
{
    public async Task ExecuteAsync(string refresh, CancellationToken cancellation)
    {
        await tokenService.RevokeAsync(refresh, cancellation);
    }
}
