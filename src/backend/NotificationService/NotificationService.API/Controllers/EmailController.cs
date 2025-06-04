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
    IVerifyConfirmationUseCase verifyConfirmationUseCase,
    IVerifyResetPasswordUseCase verifyResetPasswordUseCase,
    ITokenService tokenService,
    ILogger<EmailController> logger) : ControllerBase
{
    [HttpPost("email-confirmation")]
    public async Task<IActionResult> SendConfirmation([EmailAddress] string email, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Send confirmation in email controller");

        await sendEmailConfirmationUseCase.ExecuteAsync(email, cancellationToken);

        return Ok();
    }

    [HttpGet("confirm-email")]
    public async Task<IActionResult> ConfirmEmail(string token, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Check confirmation in email controller");

        await verifyConfirmationUseCase.ExecuteAsync(token, cancellationToken);
        return Ok();
    }

    [HttpPost("forgot-password")]
    public async Task<IActionResult> ForgotPassword([EmailAddress] string email, string newPassword, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Send reset password in email controller");
        await sendResetPasswordEmailUseCase.ExecuteAsync(email, newPassword, cancellationToken);
        return Ok();
    }

    [HttpGet("reset-password")]
    public async Task<IActionResult> ResetPassword(string token, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Check resetting in email controller");

        await verifyResetPasswordUseCase.ExecuteAsync(token, cancellationToken);
        return Ok();
    }
}
