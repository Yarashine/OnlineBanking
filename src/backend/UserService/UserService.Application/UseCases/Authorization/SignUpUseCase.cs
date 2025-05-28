using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using UserService.Application.Contracts.Services;
using UserService.Application.Contracts.UseCases.Authorization;
using UserService.Application.DTOs.Requests;
using UserService.Application.DTOs.Responses;
using UserService.Domain.Constants;
using UserService.Domain.Entities;
using UserService.Domain.Exceptions;

namespace UserService.Application.UseCases.Authorization;

public class SignUpUseCase(IMapper autoMapper, UserManager<User> userManager, ITokenService tokenService) : ISignUpUseCase
{
    public async Task<TokensResponse> ExecuteAsync(SignUpRequest request, CancellationToken cancellationToken)
    {
        var user = autoMapper.Map<User>(request);
        var result = await userManager.CreateAsync(user, request.Password);
        if (result.Errors.Any())
        {
            var error = result.Errors.First();
            var (code, message) = MapIdentityError(error);
            if (code == "USERNAME_EXISTS" || code == "EMAIL_EXISTS")
            {
                throw new AlreadyExistsException(message);
            }
            else
            {
                throw new BadRequestException(message);
            }
        }

        await userManager.AddToRoleAsync(user, Role.Client.ToString());

        var tokens = await tokenService.GenerateTokensAsync(user.Id.ToString(), request.DeviceId, Role.Client.ToString(), cancellationToken);
        var response = new TokensResponse()
        {
            AccessToken = tokens.AccessToken,
            RefreshToken = tokens.RefreshToken,
        };
        return response;
    }

    private static (string Code, string Message) MapIdentityError(IdentityError error)
    {
        return error.Code switch
        {
            "DuplicateUserName" => ("USERNAME_EXISTS", "Username already taken"),
            "DuplicateEmail" => ("EMAIL_EXISTS", "Email already registered"),
            _ => ("UNKNOWN_ERROR", error.Description)
        };
    }
}
