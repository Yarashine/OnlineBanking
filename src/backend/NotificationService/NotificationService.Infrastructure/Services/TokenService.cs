namespace NotificationService.Infrastructure.Services;

using Microsoft.Extensions.Logging;
using NotificationService.Application.Contracts.Services;
using NotificationService.Domain.Exceptions;
using NotificationService.Domain.Models;
using NotificationService.Infrastructure.Contracts.Services;
using StackExchange.Redis;
using System.Text.Json;

public class TokenService(
    IConnectionMultiplexer redis,
    IGrpcNotificationService grpcNotificationService,
    ILogger<TokenService> logger) : ITokenService
{
    private readonly IDatabase db = redis.GetDatabase();

    public async Task<string> GenerateConfirmationTokenAsync(string email, CancellationToken cancellation)
    {
        cancellation.ThrowIfCancellationRequested();

        logger.LogInformation("Before request grpc in notification service");

        var confirmToken = await grpcNotificationService.GenerateConfirmationTokenAsync(email, cancellation);

        logger.LogInformation($"After request grpc in notification service +{confirmToken}+");

        var safeToken = confirmToken.Replace("+", "_");

        await db.StringSetAsync($"confirm:{safeToken}", email, TimeSpan.FromMinutes(10));

        return safeToken;
    }

    public async Task<string> VerifyConfirmationTokenAsync(string token, CancellationToken cancellation)
    {
        cancellation.ThrowIfCancellationRequested();

        logger.LogInformation("Cherk token in notification service");

        var realToken = token.Replace("_", "+");

        var value = await db.StringGetAsync($"confirm:{token}");
        if (value.IsNullOrEmpty)
        {
            throw new BadRequestException($"Bad token exception +{token}+");
        }

        return value.ToString();
    }

    public async Task<string> GenerateResetTokenAsync(string email, string newPassword, CancellationToken cancellation)
    {
        cancellation.ThrowIfCancellationRequested();

        logger.LogInformation("Before request grpc in notification service");

        var resetToken = await grpcNotificationService.GenerateConfirmationTokenAsync(email, cancellation);

        logger.LogInformation("After request grpc in notification service");

        var resetData = new ResetData()
        {
            Email = email,
            NewPassword = newPassword,
        };

        var serializedData = JsonSerializer.Serialize(resetData);

        await db.StringSetAsync($"reset:{resetToken}", serializedData, TimeSpan.FromMinutes(10));

        return resetToken;
    }

    public async Task<ResetData> VerifyResetTokenAsync(string token, CancellationToken cancellation)
    {
        cancellation.ThrowIfCancellationRequested();

        var value = await db.StringGetAsync($"reset:{token}");
        if (value.IsNullOrEmpty)
        {
            throw new BadRequestException("Bad token exception");
        }

        var resetData = JsonSerializer.Deserialize<ResetData>(value);
        if (resetData == null || string.IsNullOrEmpty(resetData.Email) || string.IsNullOrEmpty(resetData.NewPassword))
        {
            throw new BadRequestException("Corrupted token data");
        }

        return resetData;
    }
}