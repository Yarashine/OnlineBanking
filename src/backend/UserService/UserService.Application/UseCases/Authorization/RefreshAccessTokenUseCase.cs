using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using UserService.Application.Contracts.Services;
using UserService.Application.Contracts.UseCases.Authorization;
using UserService.Application.DTOs.Requests;
using UserService.Application.DTOs.Responses;
using UserService.Domain.Entities;
using UserService.Domain.Exceptions;

namespace UserService.Application.UseCases.Authorization;

public class RefreshAccessTokenUseCase(
    UserManager<User> userManager,
    ITokenService tokenService,
    ILogger<RefreshAccessTokenUseCase> logger) : IRefreshAccessTokenUseCase
{
    public async Task<TokensResponse> ExecuteAsync(RefreshRequest request, CancellationToken cancellation)
    {
        logger.LogInformation("Attempt to refresh token for user: {UserId}", request.UserId);

        var user = await userManager.FindByIdAsync(request.UserId);
        if (user == null)
        {
            logger.LogWarning("Refresh token failed. User not found: {UserId}", request.UserId);
            throw new BadRequestException("Invalid credentials");
        }

        var roles = await userManager.GetRolesAsync(user);
        var (refreshToken, accessToken) = await tokenService.RefreshAccessTokenAsync(
            request.RefreshToken,
            request.UserId,
            request.DeviceId,
            roles.First(),
            cancellation);

        logger.LogInformation("Token refreshed successfully for user: {UserId}", request.UserId);

        return new TokensResponse
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
        };
    }
}

