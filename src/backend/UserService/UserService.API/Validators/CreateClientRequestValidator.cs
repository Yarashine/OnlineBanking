using FluentValidation;
using UserService.Application.DTOs.Requests;

namespace UserService.API.Validators;

public class CreateClientRequestValidator : AbstractValidator<CreateClientRequest>
{
    public CreateClientRequestValidator()
    {
        RuleFor(x => x.FirstName)
            .Length(2, 50).WithMessage("First name must be between 2 and 50 characters.")
            .Matches(@"^[a-zA-Z\s-]+$").WithMessage("First name can only contain letters, spaces, or hyphens.");

        RuleFor(x => x.LastName)
            .Length(2, 50).WithMessage("Last name must be between 2 and 50 characters.")
            .Matches(@"^[a-zA-Z\s-]+$").WithMessage("Last name can only contain letters, spaces, or hyphens.");

        RuleFor(x => x.Patronymic)
            .Length(2, 50).WithMessage("Patronymic must be between 2 and 50 characters.")
            .Matches(@"^[a-zA-Z\s-]*$").WithMessage("Patronymic can only contain letters, spaces, or hyphens.");

        RuleFor(x => x.PhoneNumber)
            .Matches(@"^\+\d{10,15}$").WithMessage("Phone number must be in international format (e.g., +1234567890).");

        RuleFor(x => x.PassportIdentifier)
            .Length(10).WithMessage("Passport identifier must be exactly 10 characters.")
            .Matches(@"^[A-Za-z0-9]+$").WithMessage("Passport identifier can only contain letters and numbers.");
    }
}
