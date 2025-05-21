namespace NotificationService.Infrastructure.Services;

using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using NotificationService.Application.Contracts.Services;
using NotificationService.Domain.Configs;

public class EmailService(IOptions<SmtpSettings> smtpSettingsOption) : IEmailService
{
    private readonly SmtpSettings smtpSettings = smtpSettingsOption.Value;

    public async Task SendConfirmEmail(string email, string emailBodyUrl, CancellationToken cancellation)
    {
        var subject = "Email confirmation";
        var emailBody = $"To confirm your email <a href=\"{emailBodyUrl}\">click here </a> ";
        await this.SendEmail(email, subject, emailBody, cancellation);
    }

    public async Task SendResetPasswordEmail(string email, string emailBodyUrl, CancellationToken cancellation)
    {
        var subject = "Password reset";
        var emailBody = $"To reset your password <a href=\"{emailBodyUrl}\">click here </a> ";
        await this.SendEmail(email, subject, emailBody, cancellation);
    }

    private async Task SendEmail(string email, string subject, string message, CancellationToken cancellation = default)
    {
        using var emailMessage = new MimeMessage();
        emailMessage.From.Add(new MailboxAddress(this.smtpSettings.Name, this.smtpSettings.Address));
        emailMessage.To.Add(new MailboxAddress(string.Empty, email));
        emailMessage.Subject = subject;
        emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
        {
            Text = message,
        };

        using var client = new SmtpClient();

        try
        {
            await client.ConnectAsync(this.smtpSettings.SmtpServer, this.smtpSettings.Port, this.smtpSettings.UseSSL);
            await client.AuthenticateAsync(this.smtpSettings.Username, this.smtpSettings.Password);
            var res1 = await client.SendAsync(emailMessage, cancellation);
        }
        finally
        {
            await client.DisconnectAsync(true);
        }
    }
}
