using FluentValidation;
using UserService.Application.DTOs.Requests;
using UserService.Domain.Validators;

namespace UserService.API.Validators;

public class UpdateClientRequestValidator : AbstractValidator<UpdateClientRequest>
{
    public UpdateClientRequestValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Client ID is required.")
            .Must(id => Guid.TryParse(id, out _)).WithMessage("Client ID must be a valid GUID.");

        RuleFor(x => x.FirstName)
            .Length(ClientValidator.MIN_PATRONYMIC_LENGHT, ClientValidator.MAX_PATRONYMIC_LENGHT)
            .When(x => x.FirstName != null)
            .WithMessage($"First name must be between {ClientValidator.MIN_PATRONYMIC_LENGHT} and {ClientValidator.MAX_PATRONYMIC_LENGHT} characters.")
            .Must(ClientValidator.FirstNameValidatiorRule)
            .When(x => x.FirstName != null)
            .WithMessage("First name can only contain letters, spaces, or hyphens.");

        RuleFor(x => x.LastName)
            .Length(ClientValidator.MIN_LAST_NAME_LENGHT, ClientValidator.MAX_LAST_NAME_LENGHT)
            .When(x => x.LastName != null)
            .WithMessage($"Last name must be between {ClientValidator.MIN_LAST_NAME_LENGHT} and {ClientValidator.MAX_LAST_NAME_LENGHT} characters.")
            .Must(ClientValidator.FirstNameValidatiorRule)
            .When(x => x.LastName != null)
            .WithMessage("Last name can only contain letters, spaces, or hyphens.");

        RuleFor(x => x.Patronymic)
            .Length(ClientValidator.MIN_PATRONYMIC_LENGHT, ClientValidator.MAX_PATRONYMIC_LENGHT)
            .When(x => !string.IsNullOrEmpty(x.Patronymic))
            .WithMessage($"Patronymic must be between {ClientValidator.MIN_PATRONYMIC_LENGHT} and {ClientValidator.MAX_PATRONYMIC_LENGHT} characters.")
            .Must(ClientValidator.FirstNameValidatiorRule)
            .When(x => !string.IsNullOrEmpty(x.Patronymic))
            .WithMessage("Patronymic can only contain letters, spaces, or hyphens.");

        RuleFor(x => x.PhoneNumber)
            .Must(ClientValidator.PhoneNumberValidatiorRule)
            .When(x => x.PhoneNumber != null)
            .WithMessage("Phone number must be in international format (e.g., +1234567890).");

        RuleFor(x => x.PassportIdentifier)
            .Length(ClientValidator.PASSPORT_IDENTIFIER_LENGHT)
            .When(x => x.PassportIdentifier != null)
            .WithMessage($"Passport identifier must be exactly {ClientValidator.PASSPORT_IDENTIFIER_LENGHT} characters.")
            .Must(ClientValidator.PassportValidatiorRule)
            .When(x => x.PassportIdentifier != null)
            .WithMessage("Passport identifier can only contain letters and numbers.");

        RuleFor(x => x.UserId)
            .GreaterThan(0).WithMessage("User ID must be a positive number.");
    }
}