using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using UserService.Application.Contracts.Repositories;
using UserService.Application.Contracts.UseCases.Clients;
using UserService.Application.DTOs.Responses;

namespace UserService.Application.UseCases.Clients;

public class GetByUserIdUseCase(IMapper autoMapper, IClientRepository clientRepository) : IGetByUserIdUseCase
{
    public async Task<ClientResponse> ExecuteAsync(int userId, CancellationToken cancellationToken = default)
    {
        var client = await clientRepository.GetByUserIdAsync(userId, cancellationToken);
        var response = autoMapper.Map<ClientResponse>(client);
        return response;
    }
}
