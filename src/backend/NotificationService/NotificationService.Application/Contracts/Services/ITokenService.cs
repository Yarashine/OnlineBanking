namespace NotificationService.Application.Contracts.Services;

public interface ITokenService
{
    Task<string> GenerateConfirmationTokenAsync(string userId, CancellationToken cancellation = default);
    Task VerifyConfirmationTokenAsync(string token, CancellationToken cancellation = default);
    Task<string> GenerateResetTokenAsync(string userId, string newPassword, CancellationToken cancellation = default);
    Task VerifyResetTokenAsync(string token, CancellationToken cancellation = default);
}