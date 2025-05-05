namespace NotificationService.Application.Contracts.UseCases.Email;

public interface ISendEmailConfirmationUseCase
{
    Task ExecuteAsync(string email, string emailBody, CancellationToken cancellation = default);
}