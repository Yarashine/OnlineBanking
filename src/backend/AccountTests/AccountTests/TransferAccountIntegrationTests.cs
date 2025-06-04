namespace AccountTests;

using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Moq;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using AutoMapper;
using Confluent.Kafka;
using AccountService.API.Controllers;
using AccountService.BLL.UseCases.Account.Commands.Transfer;
using AccountService.DAL.Contracts.Repositories;
using AccountService.DAL.Entities;
using AccountService.DAL.Exceptions;
using AccountService.Domain.Configs;
using AccountService.BLL.UseCases.Account.Commands.Delete;

public class TransferAccountIntegrationTests
{
    private readonly Mock<IMediator> mediatorMock = new();
    private readonly Mock<IUnitOfWork> unitOfWorkMock = new();
    private readonly Mock<IMapper> mapperMock = new();
    private readonly Mock<IProducer<Null, string>> kafkaProducerMock = new();
    private readonly Mock<IOptions<KafkaOptions>> kafkaOptionsMock = new();
    private readonly Mock<ILogger<DeleteAccountCommandHandler>> loggerMock = new();

    private readonly TransferAccountCommandHandler handler;
    private readonly TransferController controller;

    public TransferAccountIntegrationTests()
    {
        kafkaOptionsMock.Setup(o => o.Value).Returns(new KafkaOptions
        {
            Topics = new KafkaTopics
            {
                SendNotification = "notification-topic"
            }
        });

        handler = new TransferAccountCommandHandler(
            unitOfWorkMock.Object,
            mapperMock.Object,
            kafkaOptionsMock.Object,
            kafkaProducerMock.Object,
            loggerMock.Object);

        controller = new TransferController(mediatorMock.Object);
    }

    [Fact]
    public async Task Transfer_Should_Call_Mediator_And_Return_Ok()
    {
        var command = new TransferAccountCommand
        {
            SenderAccountId = Guid.NewGuid(),
            ReceiverAccountId = Guid.NewGuid(),
            Amount = 100
        };

        mediatorMock.Setup(m => m.Send(command, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var result = await controller.Transfer(command);

        Assert.IsType<OkResult>(result);
        mediatorMock.Verify(m => m.Send(command, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_Should_Transfer_Money_And_Send_Notification()
    {
        var sender = new Account { Id = Guid.NewGuid(), Balance = 200, UserId = 0 };
        var receiver = new Account { Id = Guid.NewGuid(), Balance = 50, UserId = 0 };
        var command = new TransferAccountCommand
        {
            SenderAccountId = sender.Id,
            ReceiverAccountId = receiver.Id,
            Amount = 100
        };
        var transfer = new Transfer
        {
            SenderAccountId = sender.Id,
            ReceiverAccountId = receiver.Id,
            Amount = 100
        };

        mapperMock.Setup(m => m.Map<Transfer>(It.IsAny<TransferAccountCommand>())).Returns(transfer);
        unitOfWorkMock.Setup(u => u.AccountRepository.GetByIdAsync(sender.Id, It.IsAny<CancellationToken>())).ReturnsAsync(sender);
        unitOfWorkMock.Setup(u => u.AccountRepository.GetByIdAsync(receiver.Id, It.IsAny<CancellationToken>())).ReturnsAsync(receiver);

        unitOfWorkMock.Setup(u => u.BeginTransactionAsync(It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
        unitOfWorkMock.Setup(u => u.TransferRepository.CreateAsync(transfer, It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
        unitOfWorkMock.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
        unitOfWorkMock.Setup(u => u.CommitTransactionAsync(It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

        kafkaProducerMock.Setup(p => p.ProduceAsync(It.IsAny<string>(), It.IsAny<Message<Null, string>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new DeliveryResult<Null, string>());

        await handler.Handle(command);

        unitOfWorkMock.Verify(u => u.AccountRepository.Update(It.Is<Account>(a => a.Id == sender.Id && a.Balance == 100), It.IsAny<CancellationToken>()), Times.Once);
        unitOfWorkMock.Verify(u => u.AccountRepository.Update(It.Is<Account>(a => a.Id == receiver.Id && a.Balance == 150), It.IsAny<CancellationToken>()), Times.Once);
        unitOfWorkMock.Verify(u => u.TransferRepository.CreateAsync(transfer, It.IsAny<CancellationToken>()), Times.Once);
        kafkaProducerMock.Verify(p => p.ProduceAsync("notification-topic", It.IsAny<Message<Null, string>>(), It.IsAny<CancellationToken>()), Times.Exactly(2));
    }

    [Fact]
    public async Task Handle_Should_Throw_When_Sender_Not_Found()
    {
        var command = new TransferAccountCommand { SenderAccountId = Guid.NewGuid() };
        unitOfWorkMock.Setup(u => u.AccountRepository.GetByIdAsync(command.SenderAccountId, It.IsAny<CancellationToken>())).ReturnsAsync((Account)null!);

        await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(command));
    }

    [Fact]
    public async Task Handle_Should_Throw_When_Receiver_Not_Found()
    {
        var sender = new Account { Id = Guid.NewGuid(), Balance = 200 };
        var command = new TransferAccountCommand
        {
            SenderAccountId = sender.Id,
            ReceiverAccountId = Guid.NewGuid(),
            Amount = 100
        };

        unitOfWorkMock.Setup(u => u.AccountRepository.GetByIdAsync(sender.Id, It.IsAny<CancellationToken>())).ReturnsAsync(sender);
        unitOfWorkMock.Setup(u => u.AccountRepository.GetByIdAsync(command.ReceiverAccountId, It.IsAny<CancellationToken>())).ReturnsAsync((Account)null!);

        await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(command));
    }

    [Fact]
    public async Task Handle_Should_Throw_When_Insufficient_Funds()
    {
        var sender = new Account { Id = Guid.NewGuid(), Balance = 50 };
        var command = new TransferAccountCommand
        {
            SenderAccountId = sender.Id,
            ReceiverAccountId = Guid.NewGuid(),
            Amount = 100
        };

        unitOfWorkMock.Setup(u => u.AccountRepository.GetByIdAsync(sender.Id, It.IsAny<CancellationToken>())).ReturnsAsync(sender);

        await Assert.ThrowsAsync<BadRequestException>(() => handler.Handle(command));
    }

    [Fact]
    public async Task Handle_Should_Rollback_Transaction_On_Exception()
    {
        var sender = new Account { Id = Guid.NewGuid(), Balance = 200, UserId = 0 };
        var receiver = new Account { Id = Guid.NewGuid(), Balance = 50, UserId = 0 };
        var command = new TransferAccountCommand
        {
            SenderAccountId = sender.Id,
            ReceiverAccountId = receiver.Id,
            Amount = 100
        };
        var transfer = new Transfer
        {
            SenderAccountId = sender.Id,
            ReceiverAccountId = receiver.Id,
            Amount = 100
        };

        mapperMock.Setup(m => m.Map<Transfer>(It.IsAny<TransferAccountCommand>())).Returns(transfer);
        unitOfWorkMock.Setup(u => u.AccountRepository.GetByIdAsync(sender.Id, It.IsAny<CancellationToken>())).ReturnsAsync(sender);
        unitOfWorkMock.Setup(u => u.AccountRepository.GetByIdAsync(receiver.Id, It.IsAny<CancellationToken>())).ReturnsAsync(receiver);

        unitOfWorkMock.Setup(u => u.BeginTransactionAsync(It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
        unitOfWorkMock.Setup(u => u.TransferRepository.CreateAsync(transfer, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Database error"));
        unitOfWorkMock.Setup(u => u.RollbackTransactionAsync(It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

        await Assert.ThrowsAsync<Exception>(() => handler.Handle(command));

        unitOfWorkMock.Verify(u => u.RollbackTransactionAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}
