namespace NotificationService.Application.UseCases.Email;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NotificationService.Application.Contracts.Services;
using NotificationService.Application.Contracts.UseCases.Email;

public class SendResetPasswordEmailUseCase(
    IEmailService emailService,
    ITokenService tokenService,
    IConfiguration configuration,
    ILogger<SendResetPasswordEmailUseCase> logger) : ISendResetPasswordEmailUseCase
{
    public async Task ExecuteAsync(string email, string newPassword, CancellationToken cancellation)
    {
        var token = await tokenService.GenerateResetTokenAsync(email, newPassword, cancellation);

        var emailBody = string.Format(configuration["ResetPasswordUrlTemplate"], token);

        logger.LogInformation($"Generated reset token for email: {email}");

        await emailService.SendResetPasswordEmail(email, emailBody, cancellation);

        logger.LogInformation($"Reset password email sent to: {email}");
    }

}