using FluentValidation;
using UserService.Application.DTOs.Requests;
using UserService.Domain.Validators;

namespace UserService.API.Validators;

public class SignUpRequestValidator : AbstractValidator<SignUpRequest>
{
    public SignUpRequestValidator()
    {
        RuleFor(x => x.Username)
            .Length(AuthValidator.MIN_USERNAME_LENGTH, AuthValidator.MAX_USERNAME_LENGTH)
            .WithMessage($"Username must be between {AuthValidator.MIN_USERNAME_LENGTH} and {AuthValidator.MAX_USERNAME_LENGTH} characters.")
            .Must(AuthValidator.UsernameValidatorRule)
            .WithMessage("Username can only contain letters, numbers, underscores, or hyphens.");

        RuleFor(x => x.DeviceId)
            .Length(AuthValidator.MIN_DEVICE_ID_LENGTH, AuthValidator.MAX_DEVICE_ID_LENGTH)
            .WithMessage($"Device ID must be between {AuthValidator.MIN_DEVICE_ID_LENGTH} and {AuthValidator.MAX_DEVICE_ID_LENGTH} characters.")
            .Must(AuthValidator.DeviceIdValidatorRule)
            .WithMessage("Device ID must contain only letters, numbers, or hyphens.");

        RuleFor(x => x.Password)
            .MinimumLength(AuthValidator.MIN_PASSWORD_LENGTH)
            .WithMessage($"Password must be at least {AuthValidator.MIN_PASSWORD_LENGTH} characters long.")
            .Must(AuthValidator.PasswordValidatorRule)
            .WithMessage("Password must contain at least one uppercase letter, one lowercase letter, and one digit.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Invalid email format.")
            .MaximumLength(AuthValidator.MAX_EMAIL_LENGTH)
            .WithMessage($"Email must not exceed {AuthValidator.MAX_EMAIL_LENGTH} characters.");
    }
}