using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using UserService.Application.Contracts.Repositories;
using UserService.Application.Contracts.UseCases.Clients;
using UserService.Application.DTOs.Responses;

namespace UserService.Application.UseCases.Clients;

public class GetAllUseCase(IMapper autoMapper, IClientRepository clientRepository) : IGetAllUseCase
{
    public async Task<IEnumerable<ClientResponse>> ExecuteAsync(CancellationToken cancellationToken = default)
    {
        var clients = await clientRepository.GetAllAsync(cancellationToken);
        var response = autoMapper.Map<IEnumerable<ClientResponse>>(clients);
        return response;
    }
}
