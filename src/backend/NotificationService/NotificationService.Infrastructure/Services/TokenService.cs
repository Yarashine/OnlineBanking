namespace NotificationService.Infrastructure.Services;

using NotificationService.Application.Contracts.Services;
using NotificationService.Domain.Exceptions;
using StackExchange.Redis;

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

    public async Task VerifyConfirmationTokenAsync(string userId, string token, CancellationToken cancellation)
    {
        cancellation.ThrowIfCancellationRequested();

        var value = await db.StringGetAsync($"confirm:{token}");
        if (value.IsNullOrEmpty)
        {
            throw new BadRequestException("Bad token exception");
        }
    }

    public async Task<string> GenerateResetTokenAsync(string userId, CancellationToken cancellation)
    {
        cancellation.ThrowIfCancellationRequested();

        var resetToken = Guid.NewGuid().ToString();

        await db.StringSetAsync($"reset:{resetToken}", userId, TimeSpan.FromMinutes(10));

        return resetToken;
    }

    public async Task VerifyResetTokenAsync(string userId, string token, CancellationToken cancellation)
    {
        cancellation.ThrowIfCancellationRequested();

        var value = await db.StringGetAsync($"reset:{token}");
        if (value.IsNullOrEmpty)
        {
            throw new BadRequestException("Bad token exception");
        }
    }
}