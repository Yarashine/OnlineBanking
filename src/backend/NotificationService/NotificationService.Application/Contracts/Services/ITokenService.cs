namespace NotificationService.Application.Contracts.Services;

public interface ITokenService
{
    Task<string> GenerateConfirmationTokenAsync(string userId, CancellationToken cancellation = default);
    Task VerifyConfirmationTokenAsync(string userId, string token, CancellationToken cancellation = default);
    Task<string> GenerateResetTokenAsync(string userId, CancellationToken cancellation = default);
    Task VerifyResetTokenAsync(string userId, string token, CancellationToken cancellation = default);
}