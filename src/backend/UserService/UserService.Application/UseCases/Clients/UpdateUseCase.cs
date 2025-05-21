using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using UserService.Application.Contracts.Repositories;
using UserService.Application.Contracts.UseCases.Clients;
using UserService.Application.DTOs.Requests;
using UserService.Domain.Entities;

namespace UserService.Application.UseCases.Clients;

public class UpdateUseCase(IMapper autoMapper, IClientRepository clientRepository) : IUpdateUseCase
{
    public async Task ExecuteAsync(UpdateClientRequest request, CancellationToken cancellationToken = default)
    {
        var client = autoMapper.Map<Client>(request);
        await clientRepository.UpdateAsync(client, cancellationToken);
    }
}
