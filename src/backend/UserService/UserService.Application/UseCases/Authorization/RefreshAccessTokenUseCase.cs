using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using UserService.Application.Contracts.Services;
using UserService.Application.Contracts.UseCases.Authorization;
using UserService.Application.DTOs.Requests;
using UserService.Application.DTOs.Responses;
using UserService.Domain.Entities;
using UserService.Domain.Exceptions;

namespace UserService.Application.UseCases.Authorization;

public class RefreshAccessTokenUseCase(UserManager<User> userManager, ITokenService tokenService) : IRefreshAccessTokenUseCase
{
    public async Task<TokensResponse> ExecuteAsync(RefreshRequest request, CancellationToken cancellation)
    {
        var user = await userManager.FindByIdAsync(request.UserId) ?? throw new BadRequestException("Invalid credentials");
        var roles = await userManager.GetRolesAsync(user);
        var (refreshToken, accessToken) = await tokenService.RefreshAccessTokenAsync(request.RefreshToken, request.UserId, request.DeviceId, roles.First(), cancellation);
        var response = new TokensResponse()
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
        };
        return response;
    }
}
