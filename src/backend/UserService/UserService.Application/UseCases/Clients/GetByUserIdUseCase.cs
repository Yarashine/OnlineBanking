using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using UserService.Application.Contracts.Repositories;
using UserService.Application.Contracts.UseCases.Clients;
using UserService.Application.DTOs.Responses;
using UserService.Domain.Exceptions;

namespace UserService.Application.UseCases.Clients;

public class GetByUserIdUseCase(IMapper autoMapper, IClientRepository clientRepository) : IGetByUserIdUseCase
{
    public async Task<ClientResponse> ExecuteAsync(int userId, CancellationToken cancellationToken = default)
    {
        var client = await clientRepository.GetByUserIdAsync(userId, cancellationToken) ?? throw new NotFoundException("Client with this user id doesn't exist");
        var response = autoMapper.Map<ClientResponse>(client);
        return response;
    }
}
