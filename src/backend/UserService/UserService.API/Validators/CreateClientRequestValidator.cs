using FluentValidation;
using UserService.Application.DTOs.Requests;
using UserService.Domain.Validators;

namespace UserService.API.Validators;

public class CreateClientRequestValidator : AbstractValidator<CreateClientRequest>
{
    public CreateClientRequestValidator()
    {
        RuleFor(x => x.FirstName)
            .Length(ClientValidator.MIN_PATRONYMIC_LENGHT, ClientValidator.MAX_PATRONYMIC_LENGHT)
            .WithMessage($"First name must be between {ClientValidator.MIN_PATRONYMIC_LENGHT} and {ClientValidator.MAX_PATRONYMIC_LENGHT} characters.")
            .Must(ClientValidator.FirstNameValidatiorRule).WithMessage("First name can only contain letters, spaces, or hyphens.");

        RuleFor(x => x.LastName)
            .Length(ClientValidator.MIN_LAST_NAME_LENGHT, ClientValidator.MAX_LAST_NAME_LENGHT)
            .WithMessage($"Last name must be between {ClientValidator.MIN_LAST_NAME_LENGHT} and {ClientValidator.MAX_LAST_NAME_LENGHT} characters.")
            .Must(ClientValidator.FirstNameValidatiorRule)
            .WithMessage("Last name can only contain letters, spaces, or hyphens.");

        RuleFor(x => x.Patronymic)
            .Length(ClientValidator.MIN_PATRONYMIC_LENGHT, ClientValidator.MAX_PATRONYMIC_LENGHT)
            .WithMessage($"Patronymic must be between {ClientValidator.MIN_PATRONYMIC_LENGHT} and {ClientValidator.MAX_PATRONYMIC_LENGHT} characters.")
            .Must(ClientValidator.FirstNameValidatiorRule)
            .WithMessage("Patronymic can only contain letters, spaces, or hyphens.");

        RuleFor(x => x.PhoneNumber)
            .Must(ClientValidator.FirstNameValidatiorRule)
            .WithMessage("Phone number must be in international format (e.g., +1234567890).");

        RuleFor(x => x.PassportIdentifier)
            .Length(ClientValidator.PASSPORT_IDENTIFIER_LENGHT)
            .WithMessage($"Passport identifier must be exactly {ClientValidator.PASSPORT_IDENTIFIER_LENGHT} characters.")
            .Must(ClientValidator.FirstNameValidatiorRule)
            .WithMessage("Passport identifier can only contain letters and numbers.");
    }
}
