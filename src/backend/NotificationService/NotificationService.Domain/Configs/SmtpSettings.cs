namespace NotificationService.Domain.Configs;

public class SmtpSettings
{
    public const string SectionName = "Email";
    public string SmtpServer { get; set; } = string.Empty;
    public int Port { get; set; }
    public bool UseSSL { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
}