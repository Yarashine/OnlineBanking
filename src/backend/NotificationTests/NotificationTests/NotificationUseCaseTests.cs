namespace NotificationTests;

using AutoMapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using NotificationService.Application.Contracts.Repositories;
using NotificationService.Application.DTOs.Requests;
using NotificationService.Application.DTOs.Responses;
using NotificationService.Application.UseCases.Notifications;
using NotificationService.Domain.Configs;
using NotificationService.Domain.Entities;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

public class NotificationUseCaseTests
{
    [Fact]
    public async Task GetUnreadUseCase_ShouldReturnMappedNotifications()
    {
        var paginationSettings = Options.Create(new PaginationSettings { DefaultPageSize = 10 });
        var repoMock = new Mock<INotificationRepository>();
        var mapperMock = new Mock<IMapper>();
        var loggerMock = new Mock<ILogger<GetUnreadUseCase>>();

        var notifications = new List<Notification> { new Notification(), new Notification() };
        var response = new List<NotificationResponse> { new NotificationResponse(), new NotificationResponse() };

        repoMock.Setup(r => r.GetUnreadAsync(1, 1, 2, It.IsAny<CancellationToken>())).ReturnsAsync(notifications);
        mapperMock.Setup(m => m.Map<List<NotificationResponse>>(notifications)).Returns(response);

        var useCase = new GetUnreadUseCase(paginationSettings, repoMock.Object, mapperMock.Object, loggerMock.Object);

        var result = await useCase.ExecuteAsync(1, 1, 2, CancellationToken.None);

        Assert.Equal(2, result.Count);
        repoMock.Verify(r => r.GetUnreadAsync(1, 1, 2, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetUnreadCountUseCase_ShouldReturnCount()
    {
        var repoMock = new Mock<INotificationRepository>();
        var loggerMock = new Mock<ILogger<GetUnreadCountUseCase>>();

        repoMock.Setup(r => r.GetUnreadCountAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(5);

        var useCase = new GetUnreadCountUseCase(repoMock.Object, loggerMock.Object);

        var result = await useCase.ExecuteAsync(1, CancellationToken.None);

        Assert.Equal(5, result);
        repoMock.Verify(r => r.GetUnreadCountAsync(1, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetAllUseCase_ShouldReturnMappedNotifications()
    {
        var paginationSettings = Options.Create(new PaginationSettings { DefaultPageSize = 5 });
        var repoMock = new Mock<INotificationRepository>();
        var mapperMock = new Mock<IMapper>();
        var loggerMock = new Mock<ILogger<GetAllUseCase>>();

        var notifications = new List<Notification> { new Notification() };
        var response = new List<NotificationResponse> { new NotificationResponse() };

        repoMock.Setup(r => r.GetAllAsync(1, 1, 5, It.IsAny<CancellationToken>())).ReturnsAsync(notifications);
        mapperMock.Setup(m => m.Map<List<NotificationResponse>>(notifications)).Returns(response);

        var useCase = new GetAllUseCase(paginationSettings, repoMock.Object, mapperMock.Object, loggerMock.Object);

        var result = await useCase.ExecuteAsync(1, 1, 0, CancellationToken.None);

        Assert.Single(result);
        repoMock.Verify(r => r.GetAllAsync(1, 1, 5, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task CreateUseCase_ShouldCallAddAsync()
    {
        var mapperMock = new Mock<IMapper>();
        var repoMock = new Mock<INotificationRepository>();
        var loggerMock = new Mock<ILogger<CreateUseCase>>();

        var request = new CreateNotificationRequest
        {
            UserId = 1,
            Message = "Test message"
        };

        var entity = new Notification
        {
            UserId = 1,
            Message = "Test message"
        };

        mapperMock
            .Setup(m => m.Map<Notification>(request))
            .Returns(entity);

        var useCase = new CreateUseCase(
            mapperMock.Object,
            repoMock.Object,
            loggerMock.Object
        );

        await useCase.ExecuteAsync(request, CancellationToken.None);

        repoMock.Verify(r => r.AddAsync(entity, It.IsAny<CancellationToken>()), Times.Once);
    }
}
