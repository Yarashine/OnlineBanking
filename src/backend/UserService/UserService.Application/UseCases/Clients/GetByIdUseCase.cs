using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.Internal.Mappers;
using UserService.Application.Contracts.Repositories;
using UserService.Application.Contracts.UseCases.Clients;
using UserService.Application.DTOs.Responses;
using UserService.Domain.Exceptions;

namespace UserService.Application.UseCases.Clients;

public class GetByIdUseCase(IMapper autoMapper, IClientRepository clientRepository) : IGetByIdUseCase
{
    public async Task<ClientResponse> ExecuteAsync(string id, CancellationToken cancellationToken = default)
    {
        var client = await clientRepository.GetByIdAsync(id, cancellationToken) ?? throw new NotFoundException("Client with this id doesn't exist");
        var response = autoMapper.Map<ClientResponse>(client);
        return response;
    }
}
