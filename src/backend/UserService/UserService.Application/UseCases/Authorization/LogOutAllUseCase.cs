﻿using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using UserService.Application.Contracts.Services;
using UserService.Application.Contracts.UseCases.Authorization;
using UserService.Domain.Entities;
using UserService.Domain.Exceptions;

namespace UserService.Application.UseCases.Authorization;

public class LogOutAllUseCase(UserManager<User> userManager, ITokenService tokenService) : ILogOutAllUseCase
{
    public async Task ExecuteAsync(string userId, CancellationToken cancellation)
    {
        var clientById = await userManager.FindByIdAsync(userId) ?? throw new NotFoundException("User with this id doesn't exist");
        await tokenService.RevokeAllAsync(userId, cancellation);
    }
}
