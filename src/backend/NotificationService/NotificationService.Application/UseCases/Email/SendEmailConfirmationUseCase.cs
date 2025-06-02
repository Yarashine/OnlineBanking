using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NotificationService.Application.Contracts.Services;
using NotificationService.Application.Contracts.UseCases.Email;
namespace NotificationService.Application.UseCases.Email;

public class SendEmailConfirmationUseCase(
    IEmailService emailService, 
    ITokenService tokenService,
    IConfiguration configuration,
    ILogger<SendEmailConfirmationUseCase> logger) : ISendEmailConfirmationUseCase
{
    public async Task ExecuteAsync(string email, CancellationToken cancellation)
    {
        var token = await tokenService.GenerateConfirmationTokenAsync(email, cancellation);
        var emailBody = string.Format(configuration["ConfirmEmailUrlTemplate"], token);

        logger.LogInformation($"Generated confirmation token for email: {email}");

        await emailService.SendConfirmEmail(email, emailBody, cancellation);

        logger.LogInformation($"Confirmation email sent to: {email}");
    }

}