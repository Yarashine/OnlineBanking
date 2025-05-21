namespace UserService.Domain.Validators;

public static class AuthValidator
{
    public const int MIN_USERNAME_LENGTH = 3;
    public const int MAX_USERNAME_LENGTH = 50;
    public const int MAX_EMAIL_LENGTH = 100;
    public const int MIN_PASSWORD_LENGTH = 8;
    public const int MIN_DEVICE_ID_LENGTH = 1;
    public const int MAX_DEVICE_ID_LENGTH = 100;
    public const int MAX_REFRESH_TOKEN_LENGTH = 500;

    public static bool UsernameValidatorRule(string value)
    {
        return string.IsNullOrEmpty(value) || System.Text.RegularExpressions.Regex.IsMatch(value, @"^[a-zA-Z0-9_-]+$");
    }

    public static bool DeviceIdValidatorRule(string value)
    {
        return string.IsNullOrEmpty(value) || System.Text.RegularExpressions.Regex.IsMatch(value, @"^[a-zA-Z0-9-]+$");
    }

    public static bool PasswordValidatorRule(string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return true;
        }

        return System.Text.RegularExpressions.Regex.IsMatch(value, @"[A-Z]") &&
               System.Text.RegularExpressions.Regex.IsMatch(value, @"[a-z]") &&
               System.Text.RegularExpressions.Regex.IsMatch(value, @"[0-9]");
    }
}