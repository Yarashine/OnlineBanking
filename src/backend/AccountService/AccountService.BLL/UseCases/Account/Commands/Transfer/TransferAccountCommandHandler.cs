using AccountService.BLL.UseCases.Account.Commands.Delete;
using AccountService.DAL.Contracts.Repositories;
using AccountService.DAL.Exceptions;
using AccountService.DAL.Models;
using AccountService.DAL.Repositories;
using AccountService.Domain.Configs;
using AutoMapper;
using Confluent.Kafka;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace AccountService.BLL.UseCases.Account.Commands.Transfer;

public class TransferAccountCommandHandler(
    IUnitOfWork unitOfWork,
    IMapper autoMapper,
    IOptions<KafkaOptions> options,
    IProducer<Null, string> kafkaProducer,
    ILogger<DeleteAccountCommandHandler> logger) : IRequestHandler<TransferAccountCommand>
{
    public async Task Handle(TransferAccountCommand request, CancellationToken cancellationToken = default)
    {
        var transfer = autoMapper.Map<DAL.Entities.Transfer>(request);

        var sender = await unitOfWork.AccountRepository.GetByIdAsync(request.SenderAccountId, cancellationToken) ?? throw new NotFoundException("Sender account not found.");
        if (sender.Balance < request.Amount)
        {
            throw new BadRequestException("Insufficient funds in sender account.");
        }

        var receiver = await unitOfWork.AccountRepository.GetByIdAsync(request.ReceiverAccountId, cancellationToken) ?? throw new NotFoundException("Receiver account not found.");
        await unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            sender.Balance -= transfer.Amount;
            sender.UpdatedAt = DateTime.UtcNow;

            receiver.Balance += transfer.Amount;
            receiver.UpdatedAt = DateTime.UtcNow;

            unitOfWork.AccountRepository.Update(sender, cancellationToken);
            unitOfWork.AccountRepository.Update(receiver, cancellationToken);
            await unitOfWork.TransferRepository.CreateAsync(transfer, cancellationToken);

            await unitOfWork.SaveChangesAsync(cancellationToken);
            await unitOfWork.CommitTransactionAsync(cancellationToken);
        }
        catch
        {
            await unitOfWork.RollbackTransactionAsync(cancellationToken);
            throw;
        }


        logger.LogInformation("Before sending transfer notification to notificatiion service kafka account service");

        var messageText = $"You send {request.Amount} from your account with Id {request.SenderAccountId} to {request.ReceiverAccountId}";

        var message = new SendNotificationModel()
        {
            UserId = sender.UserId,
            Message = messageText,
        };
        var serialisedMessage = JsonSerializer.Serialize(message);

        await kafkaProducer.ProduceAsync(options.Value.Topics.SendNotification, new Message<Null, string> { Value = serialisedMessage }, cancellationToken);

        messageText = $"You get {request.Amount} to your account with Id {request.ReceiverAccountId} from {request.SenderAccountId}";

        message = new SendNotificationModel()
        {
            UserId = sender.UserId,
            Message = messageText,
        };

        serialisedMessage = JsonSerializer.Serialize(message);

        await kafkaProducer.ProduceAsync(options.Value.Topics.SendNotification, new Message<Null, string> { Value = serialisedMessage }, cancellationToken);

        logger.LogInformation("After sending transfer notification to notificatiion service kafka account service");
    }
}