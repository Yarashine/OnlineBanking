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
    public async Task<IActionResult> SendConfirmation([EmailAddress] string email, int userId, CancellationToken cancellationToken = default)
    {
        var token = await tokenService.GenerateConfirmationTokenAsync(userId.ToString(), cancellationToken);
        var emailBodyUrl = Request.Scheme + "://" + Request.Host +
            Url.Action("ConfirmEmail", "Email", new {token = token });
        await sendEmailConfirmationUseCase.ExecuteAsync(email, emailBodyUrl, cancellationToken);
        return Ok();
    }

    [HttpGet("confirm-email")]
    public async Task<IActionResult> ConfirmEmail(string token, CancellationToken cancellationToken = default)
    {
        await tokenService.VerifyConfirmationTokenAsync(token, cancellationToken);
        return Ok();
    }

    [HttpPost("forgot-password")]
    public async Task<IActionResult> ForgotPassword([EmailAddress] string email, string newPassword, int userId, CancellationToken cancellationToken = default)
    {
        var token = await tokenService.GenerateResetTokenAsync(userId.ToString(), newPassword, cancellationToken);
        var emailBodyUrl = Request.Scheme + "://" + Request.Host +
            Url.Action("ResetPassword", "Email", new {token = token });
        await sendResetPasswordEmailUseCase.ExecuteAsync(email, emailBodyUrl, cancellationToken);
        return Ok();
    }

    [HttpGet("reset-password")]
    public async Task<IActionResult> ResetPassword(string token, CancellationToken cancellationToken = default)
    {
        await tokenService.VerifyResetTokenAsync(token, cancellationToken);
        return Ok();
    }
}
