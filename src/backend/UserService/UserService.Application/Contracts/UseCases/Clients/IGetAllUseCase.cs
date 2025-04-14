using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UserService.Application.DTOs.Responses;

namespace UserService.Application.Contracts.UseCases.Clients;

public interface IGetAllUseCase
{
    Task<IEnumerable<ClientResponse>> ExecuteAsync(CancellationToken cancellationToken = default);
}
