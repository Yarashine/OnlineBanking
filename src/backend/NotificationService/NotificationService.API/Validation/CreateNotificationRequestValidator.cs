namespace NotificationService.API.Validation;

using FluentValidation;
using NotificationService.Application.DTOs.Requests;
using NotificationService.Domain.Validators;

public class CreateNotificationRequestValidator : AbstractValidator<CreateNotificationRequest>
{
    public CreateNotificationRequestValidator()
    {
        RuleFor(x => x.Message)
            .Length(NotificationValidator.MIN_MESSAGE_LENGTH, NotificationValidator.MAX_MESSAGE_LENGTH)
            .WithMessage($"Message must be between {NotificationValidator.MIN_MESSAGE_LENGTH} and {NotificationValidator.MAX_MESSAGE_LENGTH} characters.");
    }
}
