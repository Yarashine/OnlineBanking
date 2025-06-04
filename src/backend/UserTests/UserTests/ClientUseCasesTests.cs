using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using UserService.Application.Contracts.Repositories;
using UserService.Application.DTOs.Requests;
using UserService.Application.DTOs.Responses;
using UserService.Application.UseCases.Clients;
using UserService.Domain.Entities;
using UserService.Domain.Exceptions;

namespace UserService.Tests.Application.UseCases.Clients
{
    public class ClientUseCasesTests
    {
        private readonly Mock<IMapper> mapperMock = new();
        private readonly Mock<IClientRepository> repoMock = new();
        private readonly Mock<ILogger<UpdateUseCase>> updateLoggerMock = new();
        private readonly Mock<ILogger<GetByUserIdUseCase>> getByUserIdLoggerMock = new();
        private readonly Mock<ILogger<GetByIdUseCase>> getByIdLoggerMock = new();
        private readonly Mock<ILogger<GetAllUseCase>> getAllLoggerMock = new();
        private readonly Mock<ILogger<DeleteUseCase>> deleteLoggerMock = new();
        private readonly Mock<ILogger<CreateUseCase>> createLoggerMock = new();

        #region UpdateUseCase Tests

        [Fact]
        public async Task UpdateUseCase_ShouldUpdate_WhenClientExists()
        {
            var request = new UpdateClientRequest { Id = "client-1" };
            var clientEntity = new Client { Id = "client-1" };
            repoMock.Setup(r => r.GetByIdAsync(request.Id, It.IsAny<CancellationToken>())).ReturnsAsync(clientEntity);
            mapperMock.Setup(m => m.Map<Client>(request)).Returns(clientEntity);

            var useCase = new UpdateUseCase(mapperMock.Object, repoMock.Object, updateLoggerMock.Object);

            await useCase.ExecuteAsync(request);

            repoMock.Verify(r => r.UpdateAsync(clientEntity, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task UpdateUseCase_ShouldThrowNotFound_WhenClientDoesNotExist()
        {
            var request = new UpdateClientRequest { Id = "client-1" };
            repoMock.Setup(r => r.GetByIdAsync(request.Id, It.IsAny<CancellationToken>())).ReturnsAsync((Client?)null);

            var useCase = new UpdateUseCase(mapperMock.Object, repoMock.Object, updateLoggerMock.Object);

            var ex = await Assert.ThrowsAsync<NotFoundException>(() => useCase.ExecuteAsync(request));
            Assert.Equal("Client with this id doesn't exist", ex.Message);
            repoMock.Verify(r => r.UpdateAsync(It.IsAny<Client>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        #endregion

        #region GetByUserIdUseCase Tests

        [Fact]
        public async Task GetByUserIdUseCase_ShouldReturnClientResponse_WhenFound()
        {
            var userId = 1;
            var clientEntity = new Client { UserId = userId };
            var responseDto = new ClientResponse();

            repoMock.Setup(r => r.GetByUserIdAsync(userId, It.IsAny<CancellationToken>())).ReturnsAsync(clientEntity);
            mapperMock.Setup(m => m.Map<ClientResponse>(clientEntity)).Returns(responseDto);

            var useCase = new GetByUserIdUseCase(mapperMock.Object, repoMock.Object, getByUserIdLoggerMock.Object);

            var result = await useCase.ExecuteAsync(userId);

            Assert.Equal(responseDto, result);
        }

        [Fact]
        public async Task GetByUserIdUseCase_ShouldThrowNotFound_WhenClientNotFound()
        {
            var userId = 1;
            repoMock.Setup(r => r.GetByUserIdAsync(userId, It.IsAny<CancellationToken>())).ReturnsAsync((Client?)null);

            var useCase = new GetByUserIdUseCase(mapperMock.Object, repoMock.Object, getByUserIdLoggerMock.Object);

            var ex = await Assert.ThrowsAsync<NotFoundException>(() => useCase.ExecuteAsync(userId));
            Assert.Equal("Client with this user id doesn't exist", ex.Message);
        }

        #endregion

        #region GetByIdUseCase Tests

        [Fact]
        public async Task GetByIdUseCase_ShouldReturnClientResponse_WhenFound()
        {
            var id = "client-1";
            var clientEntity = new Client { Id = id };
            var responseDto = new ClientResponse();

            repoMock.Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>())).ReturnsAsync(clientEntity);
            mapperMock.Setup(m => m.Map<ClientResponse>(clientEntity)).Returns(responseDto);

            var useCase = new GetByIdUseCase(mapperMock.Object, repoMock.Object, getByIdLoggerMock.Object);

            var result = await useCase.ExecuteAsync(id);

            Assert.Equal(responseDto, result);
        }

        [Fact]
        public async Task GetByIdUseCase_ShouldThrowNotFound_WhenClientNotFound()
        {
            var id = "client-1";
            repoMock.Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>())).ReturnsAsync((Client?)null);

            var useCase = new GetByIdUseCase(mapperMock.Object, repoMock.Object, getByIdLoggerMock.Object);

            var ex = await Assert.ThrowsAsync<NotFoundException>(() => useCase.ExecuteAsync(id));
            Assert.Equal("Client with this id doesn't exist", ex.Message);
        }

        #endregion

        #region GetAllUseCase Tests

        [Fact]
        public async Task GetAllUseCase_ShouldReturnClientResponses()
        {
            var clients = new List<Client> { new Client { Id = "1" }, new Client { Id = "2" } };
            var responses = new List<ClientResponse> { new ClientResponse(), new ClientResponse() };

            repoMock.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>())).ReturnsAsync(clients);
            mapperMock.Setup(m => m.Map<IEnumerable<ClientResponse>>(clients)).Returns(responses);

            var useCase = new GetAllUseCase(mapperMock.Object, repoMock.Object, getAllLoggerMock.Object);

            var result = await useCase.ExecuteAsync();

            Assert.Equal(responses, result);
        }

        #endregion

        #region DeleteUseCase Tests

        [Fact]
        public async Task DeleteUseCase_ShouldDelete_WhenClientExists()
        {
            var id = "client-1";
            var clientEntity = new Client { Id = id };

            repoMock.Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>())).ReturnsAsync(clientEntity);

            var useCase = new DeleteUseCase(repoMock.Object, deleteLoggerMock.Object);

            await useCase.ExecuteAsync(id);

            repoMock.Verify(r => r.DeleteAsync(id, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task DeleteUseCase_ShouldThrowNotFound_WhenClientDoesNotExist()
        {
            var id = "client-1";
            repoMock.Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>())).ReturnsAsync((Client?)null);

            var useCase = new DeleteUseCase(repoMock.Object, deleteLoggerMock.Object);

            var ex = await Assert.ThrowsAsync<NotFoundException>(() => useCase.ExecuteAsync(id));
            Assert.Equal("Client with this id doesn't exist", ex.Message);

            repoMock.Verify(r => r.DeleteAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        #endregion

        #region CreateUseCase Tests

        [Fact]
        public async Task CreateUseCase_ShouldCreate_WhenClientDoesNotExist()
        {
            var userId = 1;
            var request = new CreateClientRequest();

            repoMock.Setup(r => r.GetByUserIdAsync(userId, It.IsAny<CancellationToken>())).ReturnsAsync((Client?)null);

            var clientEntity = new Client();
            mapperMock.Setup(m => m.Map<Client>(request)).Returns(clientEntity);

            var useCase = new CreateUseCase(mapperMock.Object, repoMock.Object, createLoggerMock.Object);

            await useCase.ExecuteAsync(request, userId);

            repoMock.Verify(r => r.CreateAsync(It.Is<Client>(c => c == clientEntity && c.UserId == userId), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task CreateUseCase_ShouldThrowAlreadyExists_WhenClientExists()
        {
            var userId = 1;
            var request = new CreateClientRequest();

            repoMock.Setup(r => r.GetByUserIdAsync(userId, It.IsAny<CancellationToken>())).ReturnsAsync(new Client());

            var useCase = new CreateUseCase(mapperMock.Object, repoMock.Object, createLoggerMock.Object);

            var ex = await Assert.ThrowsAsync<AlreadyExistsException>(() => useCase.ExecuteAsync(request, userId));
            Assert.Equal("Client with this user id already exist", ex.Message);

            repoMock.Verify(r => r.CreateAsync(It.IsAny<Client>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        #endregion
    }
}
