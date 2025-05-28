namespace NotificationService.Infrastructure.Services;

using NotificationService.Application.Contracts.Services;
using NotificationService.Domain.Exceptions;
using StackExchange.Redis;
using System.Text.Json;

public class TokenService(IConnectionMultiplexer redis) : ITokenService
{
    private readonly IDatabase db = redis.GetDatabase();

    public async Task<string> GenerateConfirmationTokenAsync(string userId, CancellationToken cancellation)
    {
        cancellation.ThrowIfCancellationRequested();

        var confirmToken = Guid.NewGuid().ToString();

        await db.StringSetAsync($"confirm:{confirmToken}", userId, TimeSpan.FromMinutes(10));

        return confirmToken;
    }

    public async Task VerifyConfirmationTokenAsync(string token, CancellationToken cancellation)
    {
        cancellation.ThrowIfCancellationRequested();

        var value = await db.StringGetAsync($"confirm:{token}");
        if (value.IsNullOrEmpty)
        {
            throw new BadRequestException("Bad token exception");
        }
    }

    public async Task<string> GenerateResetTokenAsync(string userId, string newPassword, CancellationToken cancellation)
    {
        cancellation.ThrowIfCancellationRequested();

        var resetToken = Guid.NewGuid().ToString();

        var tokenData = new
        {
            UserId = userId,
            NewPassword = newPassword,
        };
        var serializedData = JsonSerializer.Serialize(tokenData);

        await db.StringSetAsync($"reset:{resetToken}", serializedData, TimeSpan.FromMinutes(10));

        return resetToken;
    }

    public async Task VerifyResetTokenAsync(string token, CancellationToken cancellation)
    {
        cancellation.ThrowIfCancellationRequested();

        var value = await db.StringGetAsync($"reset:{token}");
        if (value.IsNullOrEmpty)
        {
            throw new BadRequestException("Bad token exception");
        }

        var tokenData = JsonSerializer.Deserialize<TokenData>(value);
        if (tokenData == null || string.IsNullOrEmpty(tokenData.UserId) || string.IsNullOrEmpty(tokenData.HashedPassword))
        {
            throw new BadRequestException("Corrupted token data");
        }
    }

    private class TokenData
    {
        public string UserId { get; set; }
        public string HashedPassword { get; set; }
    }
}