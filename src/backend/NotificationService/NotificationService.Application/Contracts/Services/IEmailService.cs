namespace NotificationService.Application.Contracts.Services;

public interface IEmailService
{
    Task SendConfirmEmail(string email, string emailBodyUrl, CancellationToken cancellation = default);
    Task SendResetPasswordEmail(string email, string emailBodyUrl, CancellationToken cancellation = default);
}
