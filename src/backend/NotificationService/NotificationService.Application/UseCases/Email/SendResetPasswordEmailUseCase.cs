namespace NotificationService.Application.UseCases.Email;

using NotificationService.Application.Contracts.Services;
using NotificationService.Application.Contracts.UseCases.Email;

public class SendResetPasswordEmailUseCase(IEmailService emailService) : ISendResetPasswordEmailUseCase
{
    public async Task ExecuteAsync(string email, string emailBody, CancellationToken cancellation)
    {
        await emailService.SendResetPasswordEmail(email, emailBody, cancellation);
    }
}