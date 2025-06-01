using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using UserService.Application.Contracts.UseCases.Authorization;
using UserService.Domain.Entities;
using UserService.Domain.Exceptions;

namespace UserService.Application.UseCases.Authorization;

public class ConfirmEmailUseCase(
    UserManager<User> userManager,
    ILogger<ConfirmEmailUseCase> logger) : IConfirmEmailUseCase
{
    public async Task ExecuteAsync(string email, string token, CancellationToken cancellation)
    {
        logger.LogInformation("Starting email confirmation for email: {Email}", email);

        var user = await userManager.FindByEmailAsync(email);
        if (user == null)
        {
            logger.LogWarning("Email confirmation failed. User not found: {Email}", email);
            throw new NotFoundException("User with this email doesn't exist");
        }

        var result = await userManager.ConfirmEmailAsync(user, token);
        if (!result.Succeeded)
        {
            logger.LogWarning("Email confirmation failed for user: {Email}. Errors: {Errors}", email, string.Join(", ", result.Errors.Select(e => e.Description)));
            throw new BadRequestException("Email confirmation failed");
        }

        logger.LogInformation("Email confirmed successfully for user: {Email}", email);
    }
}
