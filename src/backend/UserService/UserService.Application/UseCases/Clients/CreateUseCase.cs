using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using UserService.Application.Contracts.Repositories;
using UserService.Application.Contracts.UseCases.Clients;
using UserService.Application.DTOs.Requests;
using UserService.Domain.Entities;
using UserService.Domain.Exceptions;

namespace UserService.Application.UseCases.Clients;

public class CreateUseCase(
    IMapper autoMapper,
    IClientRepository clientRepository,
    ILogger<CreateUseCase> logger) : ICreateUseCase
{
    public async Task ExecuteAsync(CreateClientRequest request, int userId, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Creating client for user id: {UserId}", userId);

        var clientByUserId = await clientRepository.GetByUserIdAsync(userId, cancellationToken);
        if (clientByUserId is not null)
        {
            logger.LogWarning("Create client failed. Client with user id {UserId} already exists.", userId);
            throw new AlreadyExistsException("Client with this user id already exist");
        }

        var client = autoMapper.Map<Client>(request);
        client.UserId = userId;

        await clientRepository.CreateAsync(client, cancellationToken);

        logger.LogInformation("Client created successfully for user id: {UserId}", userId);
    }
}

