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

public class SignInUseCase(UserManager<User> userManager, ITokenService tokenService) : ISignInUseCase
{
    public async Task<TokensResponse> ExecuteAsync(SignInRequest request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByNameAsync(request.Login) ?? throw new BadRequestException("Invalid credentials");
        var isPasswordCorrect = await userManager.CheckPasswordAsync(user, request.Password);
        if (!isPasswordCorrect)
        {
            throw new BadRequestException("Invalid credentials");
        }

        var roles = await userManager.GetRolesAsync(user);

        var tokens = await tokenService.GenerateTokensAsync(user.Id.ToString(), request.DeviceId, roles.First(), cancellationToken);
        var response = new TokensResponse()
        {
            AccessToken = tokens.AccessToken,
            RefreshToken = tokens.RefreshToken,
        };
        return response;
    }
}
