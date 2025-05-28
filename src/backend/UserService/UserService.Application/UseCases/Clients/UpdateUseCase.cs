using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using UserService.Application.Contracts.Repositories;
using UserService.Application.Contracts.UseCases.Clients;
using UserService.Application.DTOs.Requests;
using UserService.Domain.Entities;
using UserService.Domain.Exceptions;

namespace UserService.Application.UseCases.Clients;

public class UpdateUseCase(IMapper autoMapper, IClientRepository clientRepository) : IUpdateUseCase
{
    public async Task ExecuteAsync(UpdateClientRequest request, CancellationToken cancellationToken = default)
    {
        var clientById = await clientRepository.GetByIdAsync(request.Id, cancellationToken) ?? throw new NotFoundException("Client with this id doesn't exist");
        var client = autoMapper.Map<Client>(request);
        await clientRepository.UpdateAsync(client, cancellationToken);
    }
}
