namespace NotificationService.API.Controllers;

using Microsoft.AspNetCore.Mvc;
using NotificationService.Application.Contracts.Services;
using NotificationService.Application.Contracts.UseCases.Email;
using System.ComponentModel.DataAnnotations;

[ApiController]
[Route("api/email")]
public class EmailController(
    ISendEmailConfirmationUseCase sendEmailConfirmationUseCase,
    ISendResetPasswordEmailUseCase sendResetPasswordEmailUseCase,
    ITokenService tokenService) : ControllerBase
{
    [HttpPost("email-confirmation")]
    public async Task<IActionResult> SendConfirmation([EmailAddress] string email, int userId)
    {
        var token = await tokenService.GenerateConfirmationTokenAsync(userId.ToString());
        var emailBodyUrl = Request.Scheme + "://" + Request.Host +
            Url.Action("ConfirmEmail", "Email", new { email = email, token = token });
        await sendEmailConfirmationUseCase.ExecuteAsync(email, emailBodyUrl);
        return Ok();
    }

    [HttpGet("confirm-email")]
    public async Task<IActionResult> ConfirmEmail([EmailAddress] string email, string token)
    {
        await tokenService.VerifyConfirmationTokenAsync(email, token);
        return Ok();
    }

    [HttpPost("forgot-password")]
    public async Task<IActionResult> ForgotPassword([EmailAddress] string email, int userId)
    {
        var token = await tokenService.GenerateResetTokenAsync(userId.ToString());
        var emailBodyUrl = Request.Scheme + "://" + Request.Host +
            Url.Action("ResetPassword", "Email", new { email = email, token = token });
        await sendResetPasswordEmailUseCase.ExecuteAsync(email, emailBodyUrl);
        return Ok();
    }

    [HttpGet("reset-password")]
    public async Task<IActionResult> ResetPassword([EmailAddress] string email, string token)
    {
        await tokenService.VerifyResetTokenAsync(email, token);
        return Ok();
    }
}
