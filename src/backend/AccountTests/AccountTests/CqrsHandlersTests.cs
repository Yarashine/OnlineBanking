using Moq;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Confluent.Kafka;
using AccountService.BLL.UseCases.Account.Commands.Create;
using AccountService.BLL.UseCases.Account.Commands.Delete;
using AccountService.BLL.UseCases.Account.Commands.Update;
using AccountService.BLL.UseCases.Account.Queries.GetById;
using AccountService.BLL.UseCases.Account.Queries.GetAllByUserId;
using AccountService.BLL.UseCases.Account.Queries.GetAllTransfersById;
using AccountService.DAL.Contracts.Repositories;
using AccountService.DAL.Entities;
using AccountService.DAL.Exceptions;
using AccountService.Domain.Configs;
using AccountService.BLL.DTOs.Responses;
using AccountService.DAL.Configs;

namespace AccountTests;

public class CqrsHandlersTests
{
    private readonly Mock<IUnitOfWork> unitOfWorkMock = new();
    private readonly Mock<IMapper> mapperMock = new();
    private readonly Mock<IOptions<KafkaOptions>> kafkaOptionsMock = new();
    private readonly Mock<IOptions<PaginationSettings>> paginationOptionsMock = new();
    private readonly Mock<IProducer<Null, string>> producerMock = new();
    private readonly Mock<ILogger<DeleteAccountCommandHandler>> loggerMock = new();

    [Fact]

    public async Task CreateAccountCommandHandler_ShouldCreateAccountAndSendKafkaMessage()
    {
        var accountRepositoryMock = new Mock<IAccountRepository>();
        accountRepositoryMock
            .Setup(r => r.CreateAsync(It.IsAny<Account>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var unitOfWorkMock = new Mock<IUnitOfWork>();
        unitOfWorkMock
            .Setup(u => u.AccountRepository)
            .Returns(accountRepositoryMock.Object);
        unitOfWorkMock
            .Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var mapperMock = new Mock<IMapper>();

        var kafkaOptionsMock = new Mock<IOptions<KafkaOptions>>();
        kafkaOptionsMock.Setup(o => o.Value)
            .Returns(new KafkaOptions
            {
                Topics = new KafkaTopics { SendNotification = "topic" }
            });

        var producerMock = new Mock<IProducer<Null, string>>();
        producerMock
            .Setup(p => p.ProduceAsync(It.IsAny<string>(), It.IsAny<Message<Null, string>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new DeliveryResult<Null, string>());

        var loggerMock = new Mock<ILogger<DeleteAccountCommandHandler>>();

        var handler = new CreateAccountCommandHandler(
            unitOfWorkMock.Object,
            mapperMock.Object,
            kafkaOptionsMock.Object,
            producerMock.Object,
            loggerMock.Object);

        var request = new CreateAccountCommand { UserId = 123 };
        var accountEntity = new Account { Id = Guid.NewGuid(), UserId = request.UserId };

        mapperMock
            .Setup(m => m.Map<Account>(request))
            .Returns(accountEntity);

        await handler.Handle(request);

        accountRepositoryMock.Verify(r => r.CreateAsync(accountEntity, It.IsAny<CancellationToken>()), Times.Once);
        unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        producerMock.Verify(p => p.ProduceAsync("topic", It.IsAny<Message<Null, string>>(), It.IsAny<CancellationToken>()), Times.Once);

        var logMessages = loggerMock.Invocations
            .Where(i => i.Method.Name == nameof(ILogger.Log))
            .Select(i => i.Arguments[2]?.ToString())
            .ToList();

        Assert.Contains(logMessages, m => m != null && m.Contains("Before sending create notification"));
        Assert.Contains(logMessages, m => m != null && m.Contains("After sending create notification"));
    }

    [Fact]
    public async Task DeleteAccountCommandHandler_ShouldDeleteAccountAndSendKafkaMessage()
    {
        var handler = new DeleteAccountCommandHandler(unitOfWorkMock.Object, kafkaOptionsMock.Object,
            producerMock.Object, loggerMock.Object);

        var id = Guid.NewGuid();
        var entity = new Account { Id = id, UserId = 0 };

        unitOfWorkMock.Setup(u => u.AccountRepository.GetByIdAsync(id, It.IsAny<CancellationToken>())).ReturnsAsync(entity);
        kafkaOptionsMock.Setup(o => o.Value).Returns(new KafkaOptions { Topics = new KafkaTopics { SendNotification = "topic" } });

        await handler.Handle(new DeleteAccountCommand { Id = id });

        unitOfWorkMock.Verify(u => u.AccountRepository.DeleteAsync(id, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task UpdateAccountCommandHandler_ShouldUpdateAccountAndSendKafkaMessage()
    {
        var handler = new UpdateAccountCommandHandler(unitOfWorkMock.Object, mapperMock.Object,
            kafkaOptionsMock.Object, producerMock.Object, loggerMock.Object);

        var request = new UpdateAccountCommand { Id = Guid.NewGuid() };
        var entity = new Account { Id = request.Id, UserId = 0 };

        unitOfWorkMock.Setup(u => u.AccountRepository.GetByIdAsync(request.Id, It.IsAny<CancellationToken>())).ReturnsAsync(entity);
        mapperMock.Setup(m => m.Map<Account>(request)).Returns(entity);
        kafkaOptionsMock.Setup(o => o.Value).Returns(new KafkaOptions { Topics = new KafkaTopics { SendNotification = "topic" } });

        await handler.Handle(request);

        unitOfWorkMock.Verify(u => u.AccountRepository.Update(entity, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetAccountByIdQueryHandler_ShouldReturnMappedAccountResponse()
    {
        var id = Guid.NewGuid();
        var entity = new Account { Id = id };
        var response = new AccountResponse { Id = id };

        unitOfWorkMock.Setup(u => u.AccountRepository.GetByIdAsync(id, It.IsAny<CancellationToken>())).ReturnsAsync(entity);
        mapperMock.Setup(m => m.Map<AccountResponse>(entity)).Returns(response);

        var handler = new GetAccountByIdQueryHandler(unitOfWorkMock.Object, mapperMock.Object);
        var result = await handler.Handle(new GetAccountByIdQuery(id));

        Assert.Equal(id, result.Id);
    }

    [Fact]
    public async Task GetAllTransfersByIdQueryHandler_ShouldReturnMappedTransfers()
    {
        var id = Guid.NewGuid();
        var transfers = new List<Transfer>();
        var response = new List<TransferResponse>();

        unitOfWorkMock.Setup(u => u.AccountRepository.GetByIdAsync(id, It.IsAny<CancellationToken>())).ReturnsAsync(new Account());
        unitOfWorkMock.Setup(u => u.TransferRepository.GetAllByIdAsync(id, 1, 10, It.IsAny<CancellationToken>())).ReturnsAsync(transfers);
        mapperMock.Setup(m => m.Map<IEnumerable<TransferResponse>>(transfers)).Returns(response);

        paginationOptionsMock.Setup(o => o.Value).Returns(new PaginationSettings { DefaultPageSize = 10 });

        var handler = new GetAllTransfersByIdQueryHandler(unitOfWorkMock.Object, paginationOptionsMock.Object, mapperMock.Object);
        var result = await handler.Handle(new GetAllTransfersByIdQuery(id));

        Assert.Equal(response, result);
    }

    [Fact]
    public async Task GetAllAccountsByUserIdQueryHandler_ShouldReturnMappedAccounts()
    {
        var userId = 0;
        var accounts = new List<Account>();
        var response = new List<AccountResponse>();

        unitOfWorkMock.Setup(u => u.AccountRepository.GetAllByUserIdAsync(userId, 1, 10, It.IsAny<CancellationToken>())).ReturnsAsync(accounts);
        mapperMock.Setup(m => m.Map<IEnumerable<AccountResponse>>(accounts)).Returns(response);

        paginationOptionsMock.Setup(o => o.Value).Returns(new PaginationSettings { DefaultPageSize = 10 });

        var handler = new GetAllAccountsByUserIdQueryHandler(unitOfWorkMock.Object, paginationOptionsMock.Object, mapperMock.Object);
        var result = await handler.Handle(new GetAllAccountsByUserIdQuery(userId));

        Assert.Equal(response, result);
    }

    [Fact]
    public async Task GetAccountByIdQueryHandler_ShouldThrowNotFoundException_WhenAccountDoesNotExist()
    {
        var id = Guid.NewGuid();
        unitOfWorkMock.Setup(u => u.AccountRepository.GetByIdAsync(id, It.IsAny<CancellationToken>())).ReturnsAsync((Account)null);

        var handler = new GetAccountByIdQueryHandler(unitOfWorkMock.Object, mapperMock.Object);

        await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(new GetAccountByIdQuery(id)));
    }

    [Fact]
    public async Task DeleteAccountCommandHandler_ShouldThrowNotFoundException_WhenAccountNotFound()
    {
        var id = Guid.NewGuid();
        unitOfWorkMock.Setup(u => u.AccountRepository.GetByIdAsync(id, It.IsAny<CancellationToken>())).ReturnsAsync((Account)null);

        var handler = new DeleteAccountCommandHandler(unitOfWorkMock.Object, kafkaOptionsMock.Object,
            producerMock.Object, loggerMock.Object);

        await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(new DeleteAccountCommand { Id = id }));
    }
}
