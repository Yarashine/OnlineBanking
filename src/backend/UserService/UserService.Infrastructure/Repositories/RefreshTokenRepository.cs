using System.Text.Json;
using StackExchange.Redis;
using UserService.Infrastructure.RepositoryInterfaces;

namespace UserService.Infrastructure.Repositories;

public class RefreshTokenRepository(IConnectionMultiplexer redis) : IRefreshTokenRepository
{
    private readonly IDatabase db = redis.GetDatabase();

    public async Task<string> CreateAsync(string userId, string deviceId, TimeSpan expiry, CancellationToken cancellation)
    {
        cancellation.ThrowIfCancellationRequested();

        var refreshToken = Guid.NewGuid().ToString();

        var tokenData = new
        {
            UserId = userId,
            DeviceId = deviceId,
            CreatedAt = DateTime.UtcNow,
        };

        await db.StringSetAsync($"refresh:{refreshToken}", JsonSerializer.Serialize(tokenData), expiry);
        return refreshToken;
    }

    public async Task RevokeAllAsync(string userId, CancellationToken cancellation)
    {
        cancellation.ThrowIfCancellationRequested();

        var endpoints = db.Multiplexer.GetEndPoints();
        var server = db.Multiplexer.GetServer(endpoints.First());
        var keys = server.Keys(pattern: $"refresh:*");

        foreach (var key in keys)
        {
            var tokenDataJson = await db.StringGetAsync(key);
            if (!tokenDataJson.IsNullOrEmpty)
            {
                var tokenData = JsonSerializer.Deserialize<RefreshTokenData>(tokenDataJson);
                if (tokenData?.UserId == userId)
                {
                    await db.KeyDeleteAsync(key);
                }
            }
        }
    }

    public async Task RevokeAsync(string refresh, CancellationToken cancellation)
    {
        cancellation.ThrowIfCancellationRequested();
        await db.KeyDeleteAsync($"refresh:{refresh}");
    }

    public async Task<bool> ValidateAsync(string refresh, string userId, string deviceId, CancellationToken cancellation)
    {
        cancellation.ThrowIfCancellationRequested();
        var tokenDataJson = await db.StringGetAsync($"refresh:{refresh}");
        if (tokenDataJson.IsNullOrEmpty)
        {
            return false;
        }

        var tokenData = JsonSerializer.Deserialize<RefreshTokenData>(tokenDataJson);
        return tokenData?.DeviceId == deviceId && tokenData?.UserId == userId;
    }

    private record RefreshTokenData(string UserId, string DeviceId, DateTime CreatedAt);
}
