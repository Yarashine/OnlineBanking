using FluentValidation;
using UserService.Application.DTOs.Requests;

namespace UserService.API.Validators;

public class UpdateClientRequestValidator : AbstractValidator<UpdateClientRequest>
{
    public UpdateClientRequestValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Client ID is required.")
            .Matches(@"^[a-zA-Z0-9-]+$").WithMessage("Client ID must contain only letters, numbers, or hyphens.");

        RuleFor(x => x.FirstName)
            .Length(2, 50).When(x => x.FirstName != null).WithMessage("First name must be between 2 and 50 characters.")
            .Matches(@"^[a-zA-Z\s-]+$").When(x => x.FirstName != null).WithMessage("First name can only contain letters, spaces, or hyphens.");

        RuleFor(x => x.LastName)
            .Length(2, 50).When(x => x.LastName != null).WithMessage("Last name must be between 2 and 50 characters.")
            .Matches(@"^[a-zA-Z\s-]+$").When(x => x.LastName != null).WithMessage("Last name can only contain letters, spaces, or hyphens.");

        RuleFor(x => x.Patronymic)
            .Length(2, 50).WithMessage("Patronymic must be between 2 and 50 characters.")
            .Matches(@"^[a-zA-Z\s-]*$").When(x => !string.IsNullOrEmpty(x.Patronymic)).WithMessage("Patronymic can only contain letters, spaces, or hyphens.");

        RuleFor(x => x.PhoneNumber)
            .Matches(@"^\+\d{10,15}$").When(x => x.PhoneNumber != null).WithMessage("Phone number must be in international format (e.g., +1234567890).");

        RuleFor(x => x.PassportIdentifier)
            .Length(10).When(x => x.PassportIdentifier != null).WithMessage("Passport identifier must be exactly 10 characters.")
            .Matches(@"^[A-Za-z0-9]+$").When(x => x.PassportIdentifier != null).WithMessage("Passport identifier can only contain letters and numbers.");

        RuleFor(x => x.UserId)
            .GreaterThan(0).WithMessage("User ID must be a positive number.");
    }
}
