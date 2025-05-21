using System.Threading;
using System.Threading.Tasks;

namespace UserService.Application.Contracts.Services;

public interface ITokenService
{
    Task RevokeAsync(string refresh, CancellationToken cancellation = default);
    Task RevokeAllAsync(string userId, CancellationToken cancellation = default);
    Task<(string RefreshToken, string AccessToken)> GenerateTokensAsync(string userId, string deviceId, string role, CancellationToken cancellation = default);
    Task<(string RefreshToken, string AccessToken)> RefreshAccessTokenAsync(string refreshToken, string userId, string deviceId, string role, CancellationToken cancellation = default);
}
