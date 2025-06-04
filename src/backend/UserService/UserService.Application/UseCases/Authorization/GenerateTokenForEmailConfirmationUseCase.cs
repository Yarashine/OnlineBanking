using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using UserService.Application.Contracts.UseCases.Authorization;
using UserService.Domain.Entities;
using UserService.Domain.Exceptions;

namespace UserService.Application.UseCases.Authorization;

public class GenerateTokenForEmailConfirmationUseCase(
    UserManager<User> userManager,
    ILogger<GenerateTokenForEmailConfirmationUseCase> logger) : IGenerateTokenForEmailConfirmationUseCase
{
    public async Task<string> ExecuteAsync(string email, CancellationToken cancellation)
    {
        logger.LogInformation("Generating email confirmation token for email: {Email}", email);

        var user = await userManager.FindByEmailAsync(email);
        if (user == null)
        {
            logger.LogWarning("Token generation failed. User not found: {Email}", email);
            throw new NotFoundException("User with this email doesn't exist");
        }

        var token = await userManager.GenerateEmailConfirmationTokenAsync(user);

        logger.LogInformation("Email confirmation token generated successfully for user: {Email}", email);

        return token;
    }
}
