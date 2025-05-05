using NotificationService.Application.Contracts.Services;
namespace NotificationService.Application.UseCases.Email;

using NotificationService.Application.Contracts.UseCases.Email;

public class SendEmailConfirmationUseCase(IEmailService emailService) : ISendEmailConfirmationUseCase
{
    public async Task ExecuteAsync(string email, string emailBody, CancellationToken cancellation)
    {
        await emailService.SendConfirmEmail(email, emailBody, cancellation);
    }
}