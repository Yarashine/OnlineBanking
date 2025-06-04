using AccountService.BLL.UseCases.Account.Commands.Delete;
using AccountService.DAL.Contracts.Repositories;
using AccountService.DAL.Models;
using AccountService.Domain.Configs;
using AutoMapper;
using Confluent.Kafka;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace AccountService.BLL.UseCases.Account.Commands.Create;

public class CreateAccountCommandHandler(
    IUnitOfWork unitOfWork,
    IMapper autoMapper,
    IOptions<KafkaOptions> options,
    IProducer<Null, string> kafkaProducer,
    ILogger<DeleteAccountCommandHandler> logger) : IRequestHandler<CreateAccountCommand>
{
    public async Task Handle(CreateAccountCommand request, CancellationToken cancellationToken = default)
    {
        Console.WriteLine("create command");
        var account = autoMapper.Map<DAL.Entities.Account>(request);
        account.Balance = 10;
        await unitOfWork.AccountRepository.CreateAsync(account, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Before sending create notification to notificatiion service kafka account service");

        var messageText = $"You create new account with id {account.Id}";

        var message = new SendNotificationModel()
        {
            UserId = request.UserId,
            Message = messageText,
        };

        var serialisedMessage = JsonSerializer.Serialize(message);

        await kafkaProducer.ProduceAsync(options.Value.Topics.SendNotification, new Message<Null, string> { Value = serialisedMessage }, cancellationToken);

        logger.LogInformation("After sending create notification to notificatiion service kafka account service");
    }
}