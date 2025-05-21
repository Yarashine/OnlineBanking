using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using UserService.Application.Contracts.UseCases.Authorization;
using UserService.Application.DTOs.Requests;
using UserService.Application.DTOs.Responses;

namespace UserService.API.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController(
    ILogOutAllUseCase logOutAllUseCase,
    ILogOutUseCase logOutUseCase,
    IRefreshAccessTokenUseCase refreshAccessTokenUseCase,
    ISignInUseCase signInUseCase,
    ISignUpUseCase signUpUseCase) : ControllerBase
{
    [HttpPost("signup")]
    public async Task<ActionResult<TokensResponse>> SignUp(SignUpRequest request, CancellationToken cancellation = default)
    {
        var tokens = await signUpUseCase.ExecuteAsync(request, cancellation);
        return tokens;
    }

    [HttpPost("signin")]
    public async Task<ActionResult<TokensResponse>> SignIn(SignInRequest request, CancellationToken cancellation = default)
    {
        var tokens = await signInUseCase.ExecuteAsync(request, cancellation);
        return tokens;
    }

    [HttpPut("refresh")]
    public async Task<ActionResult<TokensResponse>> RefreshAccessToken(RefreshRequest request, CancellationToken cancellation = default)
    {
        var tokens = await refreshAccessTokenUseCase.ExecuteAsync(request, cancellation);
        return tokens;
    }

    [Authorize]
    [HttpDelete("logout")]
    public async Task<IActionResult> LogOut(string refresh, CancellationToken cancellation = default)
    {
        await logOutUseCase.ExecuteAsync(refresh, cancellation);
        return Ok();
    }

    [Authorize]
    [HttpDelete("logout/all")]
    public async Task<IActionResult> LogOutAll(string userId, CancellationToken cancellation = default)
    {
        await logOutAllUseCase.ExecuteAsync(userId, cancellation);
        return Ok();
    }

    [Authorize]
    [HttpGet]
    public IActionResult Protected()
    {
        return Ok();
    }
}
