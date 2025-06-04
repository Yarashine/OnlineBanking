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

public class SignInUseCase(
    UserManager<User> userManager,
    ITokenService tokenService,
    ILogger<SignInUseCase> logger) : ISignInUseCase
{
    public async Task<TokensResponse> ExecuteAsync(SignInRequest request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Sign-in attempt for login: {Login}", request.Login);

        var user = await userManager.FindByEmailAsync(request.Login);
        if (user == null)
        {
            logger.LogWarning("Sign-in failed. User not found: {Login}", request.Login);
            throw new BadRequestException("Invalid credentials");
        }

        //var isConfirmedEmail = await userManager.IsEmailConfirmedAsync(user);
        //if (!isConfirmedEmail)
        //{
        //    logger.LogWarning("Sign-in failed. Email not confirmed for user: {Login}", request.Login);
        //    throw new BadRequestException("Email should be confirmed");
        //}

        var isPasswordCorrect = await userManager.CheckPasswordAsync(user, request.Password);
        if (!isPasswordCorrect)
        {
            logger.LogWarning("Sign-in failed. Incorrect password for user: {Login}", request.Login);
            throw new BadRequestException("Invalid credentials");
        }

        var roles = await userManager.GetRolesAsync(user);

        var tokens = await tokenService.GenerateTokensAsync(user.Id.ToString(), request.DeviceId, roles.First(), cancellationToken);

        logger.LogInformation("Sign-in successful for user: {Login}", request.Login);

        return new TokensResponse
        {
            AccessToken = tokens.AccessToken,
            RefreshToken = tokens.RefreshToken,
        };
    }
}
