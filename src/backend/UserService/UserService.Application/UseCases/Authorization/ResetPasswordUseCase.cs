using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using UserService.Application.Contracts.UseCases.Authorization;
using UserService.Domain.Entities;
using UserService.Domain.Exceptions;

namespace UserService.Application.UseCases.Authorization;

public class ResetPasswordUseCase(UserManager<User> userManager) : IResetPasswordUseCase
{
    public async Task ExecuteAsync(string userId, string password, string token, CancellationToken cancellation)
    {
        var user = await userManager.FindByIdAsync(userId) ?? throw new NotFoundException("User with this id doesn't exist");
        await userManager.ResetPasswordAsync(user, token, password);
    }
}