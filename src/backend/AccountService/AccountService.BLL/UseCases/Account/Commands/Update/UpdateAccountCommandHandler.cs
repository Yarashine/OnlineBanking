using AccountService.BLL.UseCases.Account.Commands.Delete;
using AccountService.DAL.Contracts.Repositories;
using AccountService.DAL.Exceptions;
using AccountService.DAL.Models;
using AccountService.Domain.Configs;
using AutoMapper;
using Confluent.Kafka;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace AccountService.BLL.UseCases.Account.Commands.Update;

public class UpdateAccountCommandHandler(
    IUnitOfWork unitOfWork,
    IMapper autoMapper,
    IOptions<KafkaOptions> options,
    IProducer<Null, string> kafkaProducer,
    ILogger<DeleteAccountCommandHandler> logger) : IRequestHandler<UpdateAccountCommand>
{
    public async Task Handle(UpdateAccountCommand request, CancellationToken cancellationToken = default)
    {
        var accountForCheck = await unitOfWork.AccountRepository.GetByIdAsync(request.Id, cancellationToken) ?? throw new NotFoundException("Account");
        var account = autoMapper.Map<DAL.Entities.Account>(request);
        unitOfWork.AccountRepository.Update(account, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Before sending update notification to notificatiion service kafka account service");

        var messageText = $"Your account with Id {request.Id} was updated";

        var message = new SendNotificationModel()
        {
            UserId = accountForCheck.UserId,
            Message = messageText,
        };

        var serialisedMessage = JsonSerializer.Serialize(message);

        await kafkaProducer.ProduceAsync(options.Value.Topics.SendNotification, new Message<Null, string> { Value = serialisedMessage }, cancellationToken);

        logger.LogInformation("After sending update notification to notificatiion service kafka account service");
    }
}