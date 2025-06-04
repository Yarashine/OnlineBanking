using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using UserService.Application.Contracts.UseCases.Authorization;
using UserService.Domain.Entities;
using UserService.Domain.Exceptions;

namespace UserService.Application.UseCases.Authorization;

public class GenerateTokenForResetPassword(
    UserManager<User> userManager,
    ILogger<GenerateTokenForResetPassword> logger) : IGenerateTokenForResetPassword
{
    public async Task<string> ExecuteAsync(string email, CancellationToken cancellation)
    {
        logger.LogInformation("Generating password reset token for email: {Email}", email);

        var user = await userManager.FindByEmailAsync(email);
        if (user == null)
        {
            logger.LogWarning("Password reset token generation failed. User not found: {Email}", email);
            throw new NotFoundException("User with this email doesn't exist");
        }

        var token = await userManager.GeneratePasswordResetTokenAsync(user);

        logger.LogInformation("Password reset token generated successfully for user: {Email}", email);

        return token;
    }
}
