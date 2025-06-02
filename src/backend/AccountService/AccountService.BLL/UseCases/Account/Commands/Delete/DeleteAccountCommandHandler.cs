using AccountService.DAL.Contracts.Repositories;
using AccountService.DAL.Exceptions;
using AccountService.DAL.Models;
using AccountService.Domain.Configs;
using Confluent.Kafka;
using Elastic.CommonSchema;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace AccountService.BLL.UseCases.Account.Commands.Delete;

public class DeleteAccountCommandHandler(
    IUnitOfWork unitOfWork,
    IOptions<KafkaOptions> options,
    IProducer<Null, string> kafkaProducer,
    ILogger<DeleteAccountCommandHandler> logger) : IRequestHandler<DeleteAccountCommand>
{
    public async Task Handle(DeleteAccountCommand request, CancellationToken cancellationToken = default)
    {
        var accountForCheck = await unitOfWork.AccountRepository.GetByIdAsync(request.Id, cancellationToken) ?? throw new NotFoundException("Account");

        await unitOfWork.AccountRepository.DeleteAsync(request.Id, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Before sending delete notification to notificatiion service kafka account service");

        var messageText = $"Your account was deleted with id {request.Id}";

        var message = new SendNotificationModel()
        {
            UserId = accountForCheck.UserId,
            Message = messageText,
        };

        var serialisedMessage = JsonSerializer.Serialize(message);

        await kafkaProducer.ProduceAsync(options.Value.Topics.SendNotification, new Message<Null, string> { Value = serialisedMessage }, cancellationToken);

        logger.LogInformation("After sending delete notification to notificatiion service kafka account service");
    }
}