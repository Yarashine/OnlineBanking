using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using UserService.Application.Contracts.Repositories;
using UserService.Application.Contracts.UseCases.Clients;
using UserService.Application.DTOs.Responses;
using UserService.Domain.Exceptions;

namespace UserService.Application.UseCases.Clients;

public class GetByUserIdUseCase(IMapper autoMapper,
    IClientRepository clientRepository,
    ILogger<GetByUserIdUseCase> logger) : IGetByUserIdUseCase
{
    public async Task<ClientResponse> ExecuteAsync(int userId, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Getting client by user id: {UserId}", userId);

        var client = await clientRepository.GetByUserIdAsync(userId, cancellationToken);
        if (client == null)
        {
            logger.LogWarning("Client with user id {UserId} not found.", userId);
            throw new NotFoundException("Client with this user id doesn't exist");
        }

        var response = autoMapper.Map<ClientResponse>(client);
        logger.LogInformation("Client with user id {UserId} retrieved successfully.", userId);

        return response;
    }
}

