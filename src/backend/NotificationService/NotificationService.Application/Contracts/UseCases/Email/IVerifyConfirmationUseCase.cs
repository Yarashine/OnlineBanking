namespace NotificationService.Application.Contracts.UseCases.Email;

public interface IVerifyConfirmationUseCase
{
    Task ExecuteAsync(string token, CancellationToken cancellation = default);
}