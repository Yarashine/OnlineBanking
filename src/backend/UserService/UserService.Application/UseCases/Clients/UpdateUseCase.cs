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

public class UpdateUseCase(
    IMapper autoMapper,
    IClientRepository clientRepository,
    ILogger<UpdateUseCase> logger) : IUpdateUseCase
{
    public async Task ExecuteAsync(UpdateClientRequest request, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Updating client with id: {ClientId}", request.Id);

        var clientById = await clientRepository.GetByIdAsync(request.Id, cancellationToken);
        if (clientById == null)
        {
            logger.LogWarning("Update failed. Client with id {ClientId} not found.", request.Id);
            throw new NotFoundException("Client with this id doesn't exist");
        }

        var client = autoMapper.Map<Client>(request);
        await clientRepository.UpdateAsync(client, cancellationToken);

        logger.LogInformation("Client with id {ClientId} updated successfully.", request.Id);
    }
}

