using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using UserService.Application.Contracts.Repositories;
using UserService.Application.Contracts.UseCases.Clients;
using UserService.Application.DTOs.Responses;
using UserService.Domain.Exceptions;

namespace UserService.Application.UseCases.Clients;

public class GetByIdUseCase(
    IMapper autoMapper,
    IClientRepository clientRepository,
    ILogger<GetByIdUseCase> logger) : IGetByIdUseCase
{
    public async Task<ClientResponse> ExecuteAsync(string id, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Getting client by id: {ClientId}", id);

        var client = await clientRepository.GetByIdAsync(id, cancellationToken);
        if (client == null)
        {
            logger.LogWarning("Client with id {ClientId} not found.", id);
            throw new NotFoundException("Client with this id doesn't exist");
        }

        var response = autoMapper.Map<ClientResponse>(client);
        logger.LogInformation("Client with id {ClientId} retrieved successfully.", id);

        return response;
    }
}