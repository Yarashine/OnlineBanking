using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using UserService.Application.Contracts.Services;
using UserService.Infrastructure.Configs;
using UserService.Infrastructure.RepositoryInterfaces;

namespace UserService.Infrastructure.Services;

public class TokenService(IOptions<JwtOptions> jwtOptions, IRefreshTokenRepository refreshRepository) : ITokenService
{
    private readonly JwtOptions _jwtOptions = jwtOptions.Value;
    private readonly RSA rsa = RSA.Create();

    public async Task<(string RefreshToken, string AccessToken)> GenerateTokensAsync(string userId, string deviceId, string role, CancellationToken cancellation = default)
    {
        var access = GenerateAccessToken(userId, role, cancellation);
        var refresh = await GenerateRefreshTokenAsync(userId, deviceId, cancellation);
        return (refresh, access);
    }

    public async Task RevokeAsync(string refresh, CancellationToken cancellation = default)
    {
        await refreshRepository.RevokeAsync(refresh, cancellation);
    }

    public async Task RevokeAllAsync(string userId, CancellationToken cancellation = default)
    {
        await refreshRepository.RevokeAllAsync(userId, cancellation);
    }

    public async Task<(string RefreshToken, string AccessToken)> RefreshAccessTokenAsync(string refreshToken, string userId, string deviceId, string role, CancellationToken cancellation = default)
    {
        var isValidatedToken = await refreshRepository.ValidateAsync(refreshToken, userId, deviceId, cancellation);
        if (!isValidatedToken)
        {
            throw new SecurityTokenException($"Invalid refresh token or device mismatch");
        }

        await refreshRepository.RevokeAsync(refreshToken, cancellation);
        var tokens = await GenerateTokensAsync(userId, deviceId, role, cancellation);
        return tokens;
    }

    private string GenerateAccessToken(string userId, string role, CancellationToken cancellation = default)
    {
        cancellation.ThrowIfCancellationRequested();

        string jwtSecret = File.ReadAllText(_jwtOptions.PrivateKeyPath).Trim();

        rsa.ImportFromPem(jwtSecret);

        var credentials = new SigningCredentials(
            new RsaSecurityKey(rsa),
            SecurityAlgorithms.RsaSha256);

        var claims = new List<Claim>()
        {
            new (JwtRegisteredClaimNames.Sub, userId),
            new (ClaimTypes.Role, role),
        };

        var token = new JwtSecurityToken(
            issuer: _jwtOptions.Issuer,
            audience: _jwtOptions.Audience,
            notBefore: DateTime.UtcNow,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_jwtOptions.ExpiryAccessInMinutes),
            signingCredentials: credentials);

        var writedToken = new JwtSecurityTokenHandler().WriteToken(token);
        return writedToken;
    }

    private async Task<string> GenerateRefreshTokenAsync(string userId, string deviceId, CancellationToken cancellation = default)
    {
        var refresh = await refreshRepository.CreateAsync(userId, deviceId, TimeSpan.FromDays(_jwtOptions.ExpiryRefreshInDays), cancellation);

        return refresh;
    }
}
