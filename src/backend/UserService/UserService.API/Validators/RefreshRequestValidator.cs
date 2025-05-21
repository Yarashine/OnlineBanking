using FluentValidation;
using UserService.Application.DTOs.Requests;
using UserService.Domain.Validators;

namespace UserService.API.Validators;

public class RefreshRequestValidator : AbstractValidator<RefreshRequest>
{
    public RefreshRequestValidator()
    {
        RuleFor(x => x.RefreshToken)
            .NotEmpty().WithMessage("Refresh token is required.")
            .MaximumLength(AuthValidator.MAX_REFRESH_TOKEN_LENGTH)
            .WithMessage($"Refresh token must not exceed {AuthValidator.MAX_REFRESH_TOKEN_LENGTH} characters.");

        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("User ID is required.")
            .Must(id => int.TryParse(id, out var userId) && userId > 0)
            .WithMessage("User ID must be a positive integer.");

        RuleFor(x => x.DeviceId)
            .Length(AuthValidator.MIN_DEVICE_ID_LENGTH, AuthValidator.MAX_DEVICE_ID_LENGTH)
            .WithMessage($"Device ID must be between {AuthValidator.MIN_DEVICE_ID_LENGTH} and {AuthValidator.MAX_DEVICE_ID_LENGTH} characters.")
            .Must(AuthValidator.DeviceIdValidatorRule)
            .WithMessage("Device ID must contain only letters, numbers, or hyphens.");
    }
}