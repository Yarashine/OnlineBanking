using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using UserService.Application.Contracts.Repositories;
using UserService.Application.Contracts.UseCases.Clients;
using UserService.Application.DTOs.Responses;

namespace UserService.Application.UseCases.Clients;

public class GetByIdUseCase(IMapper autoMapper, IClientRepository clientRepository) : IGetByIdUseCase
{
    public async Task<ClientResponse> ExecuteAsync(string id, CancellationToken cancellationToken = default)
    {
        var client = await clientRepository.GetByIdAsync(id, cancellationToken);
        var response = autoMapper.Map<ClientResponse>(client);
        return response;
    }
}
