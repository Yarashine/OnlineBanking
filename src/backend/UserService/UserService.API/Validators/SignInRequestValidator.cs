using FluentValidation;
using UserService.Application.DTOs.Requests;
using UserService.Domain.Validators;

namespace UserService.API.Validators;

public class SignInRequestValidator : AbstractValidator<SignInRequest>
{
    public SignInRequestValidator()
    {
        RuleFor(x => x.Login)
            .NotEmpty().WithMessage("Login is required.")
            .MaximumLength(AuthValidator.MAX_EMAIL_LENGTH)
            .WithMessage($"Login must not exceed {AuthValidator.MAX_EMAIL_LENGTH} characters.");

        RuleFor(x => x.Password)
            .MinimumLength(AuthValidator.MIN_PASSWORD_LENGTH)
            .WithMessage($"Password must be at least {AuthValidator.MIN_PASSWORD_LENGTH} characters long.")
            .Must(AuthValidator.PasswordValidatorRule)
            .WithMessage("Password must contain at least one uppercase letter, one lowercase letter, and one digit.");
    }
}
