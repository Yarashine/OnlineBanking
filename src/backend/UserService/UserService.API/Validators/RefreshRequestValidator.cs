using FluentValidation;
using UserService.Application.DTOs.Requests;

namespace UserService.API.Validators;

public class RefreshRequestValidator : AbstractValidator<RefreshRequest>
{
    public RefreshRequestValidator()
    {
        RuleFor(x => x.RefreshToken)
            .NotEmpty().WithMessage("Refresh token is required.")
            .MaximumLength(500).WithMessage("Refresh token must not exceed 500 characters.");

        RuleFor(x => x.DeviceId)
            .NotEmpty().WithMessage("Device ID is required.")
            .Matches(@"^[a-zA-Z0-9-]+$").WithMessage("Device ID must contain only letters, numbers, or hyphens.")
            .Length(1, 100).WithMessage("Device ID must be between 1 and 100 characters.");

        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("User ID is required.")
            .Must(id => int.TryParse(id, out var userId) && userId > 0)
            .WithMessage("User ID must be a positive integer.");
    }
}