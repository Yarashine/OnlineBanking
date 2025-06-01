using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using UserService.Application.Contracts.Services;
using UserService.Application.Contracts.UseCases.Authorization;

namespace UserService.Application.UseCases.Authorization;

public class LogOutUseCase(
    ITokenService tokenService,
    ILogger<LogOutUseCase> logger) : ILogOutUseCase
{
    public async Task ExecuteAsync(string refresh, CancellationToken cancellation)
    {
        logger.LogInformation("Revoking refresh token");
        await tokenService.RevokeAsync(refresh, cancellation);
        logger.LogInformation("Refresh token successfully revoked");
    }
}

