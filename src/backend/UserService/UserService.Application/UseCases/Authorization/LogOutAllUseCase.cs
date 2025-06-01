using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using UserService.Application.Contracts.Services;
using UserService.Application.Contracts.UseCases.Authorization;
using UserService.Domain.Entities;
using UserService.Domain.Exceptions;

namespace UserService.Application.UseCases.Authorization;

public class LogOutAllUseCase(
    UserManager<User> userManager,
    ITokenService tokenService,
    ILogger<LogOutAllUseCase> logger) : ILogOutAllUseCase
{
    public async Task ExecuteAsync(string userId, CancellationToken cancellation)
    {
        logger.LogInformation("Log out all sessions request for user id: {UserId}", userId);

        var user = await userManager.FindByIdAsync(userId);
        if (user == null)
        {
            logger.LogWarning("Log out all sessions failed. User not found with id: {UserId}", userId);
            throw new NotFoundException("User with this id doesn't exist");
        }

        await tokenService.RevokeAllAsync(userId, cancellation);

        logger.LogInformation("All sessions revoked successfully for user id: {UserId}", userId);
    }
}

