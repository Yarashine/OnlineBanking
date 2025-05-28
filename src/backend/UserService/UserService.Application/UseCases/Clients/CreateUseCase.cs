using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using UserService.Application.Contracts.Repositories;
using UserService.Application.Contracts.UseCases.Clients;
using UserService.Application.DTOs.Requests;
using UserService.Domain.Entities;
using UserService.Domain.Exceptions;

namespace UserService.Application.UseCases.Clients;

public class CreateUseCase(IMapper autoMapper, IClientRepository clientRepository) : ICreateUseCase
{
    public async Task ExecuteAsync(CreateClientRequest request, int userId, CancellationToken cancellationToken = default)
    {
        var clientByUserId = await clientRepository.GetByUserIdAsync(userId, cancellationToken);
        if (clientByUserId is not null)
        {
            throw new AlreadyExistsException("Client with this user id already exist");
        }

        var client = autoMapper.Map<Client>(request);
        client.UserId = userId;

        await clientRepository.CreateAsync(client, cancellationToken);
    }
}
