namespace UserService.Infrastructure.RepositoryInterfaces;

public interface IRefreshTokenRepository
{
    Task<string> CreateAsync(string userId, string deviceId, TimeSpan expiry, CancellationToken cancellation = default);
    Task<bool> ValidateAsync(string refresh, string userId, string deviceId, CancellationToken cancellation = default);
    Task RevokeAsync(string refresh, CancellationToken cancellation = default);
    Task RevokeAllAsync(string userId, CancellationToken cancellation = default);
}
