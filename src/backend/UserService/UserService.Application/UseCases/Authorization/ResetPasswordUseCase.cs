using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using UserService.Application.Contracts.UseCases.Authorization;
using UserService.Domain.Entities;
using UserService.Domain.Exceptions;

namespace UserService.Application.UseCases.Authorization;

public class ResetPasswordUseCase(
    UserManager<User> userManager,
    ILogger<ResetPasswordUseCase> logger) : IResetPasswordUseCase
{
    public async Task ExecuteAsync(string email, string password, string token, CancellationToken cancellation)
    {
        logger.LogInformation("Reset password attempt for email: {Email}", email);

        var user = await userManager.FindByEmailAsync(email);
        if (user == null)
        {
            logger.LogWarning("Reset password failed. User not found: {Email}", email);
            throw new NotFoundException("User with this email doesn't exist");
        }

        await userManager.ResetPasswordAsync(user, token, password);

        logger.LogInformation("Password successfully reset for user: {Email}", email);
    }
}
