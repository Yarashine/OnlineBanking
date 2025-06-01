using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using UserService.Application.Contracts.Repositories;
using UserService.Application.Contracts.UseCases.Clients;
using UserService.Application.DTOs.Responses;

namespace UserService.Application.UseCases.Clients;

public class GetAllUseCase(
    IMapper autoMapper,
    IClientRepository clientRepository,
    ILogger<GetAllUseCase> logger) : IGetAllUseCase
{
    public async Task<IEnumerable<ClientResponse>> ExecuteAsync(CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Getting all clients");

        var clients = await clientRepository.GetAllAsync(cancellationToken);
        var response = autoMapper.Map<IEnumerable<ClientResponse>>(clients);

        logger.LogInformation("Retrieved {Count} clients", response?.Count() ?? 0);

        return response;
    }
}

