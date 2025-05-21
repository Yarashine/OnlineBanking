using System;
using System.Threading;
using System.Threading.Tasks;
using UserService.Application.Contracts.Services;
using UserService.Application.Contracts.UseCases.Authorization;

namespace UserService.Application.UseCases.Authorization;

public class LogOutAllUseCase(ITokenService tokenService) : ILogOutAllUseCase
{
    public async Task ExecuteAsync(string userId, CancellationToken cancellation)
    {
        await tokenService.RevokeAllAsync(userId, cancellation);
    }
}
