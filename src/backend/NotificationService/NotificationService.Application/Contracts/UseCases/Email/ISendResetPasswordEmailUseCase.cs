namespace NotificationService.Application.Contracts.UseCases.Email;

public interface ISendResetPasswordEmailUseCase
{
    Task ExecuteAsync(string email, string emailBody, CancellationToken cancellation = default);
}