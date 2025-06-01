using NotificationService.Domain.Models;

namespace NotificationService.Application.Contracts.Services;

public interface ITokenService
{
    Task<string> GenerateConfirmationTokenAsync(string email, CancellationToken cancellation = default);
    Task<string> VerifyConfirmationTokenAsync(string token, CancellationToken cancellation = default);
    Task<string> GenerateResetTokenAsync(string email, string newPassword, CancellationToken cancellation = default);
    Task<ResetData> VerifyResetTokenAsync(string token, CancellationToken cancellation = default);
}