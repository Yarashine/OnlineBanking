using FluentValidation;
using UserService.Application.DTOs.Requests;
using UserService.Domain.Validators;

namespace UserService.API.Validators;

public class SignUpRequestValidator : AbstractValidator<SignUpRequest>
{
    public SignUpRequestValidator()
    {
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