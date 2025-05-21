using System.Threading;
using System.Threading.Tasks;
using UserService.Application.DTOs.Requests;
using UserService.Application.DTOs.Responses;

namespace UserService.Application.Contracts.UseCases.Authorization;

public interface IRefreshAccessTokenUseCase
{
    Task<TokensResponse> ExecuteAsync(RefreshRequest request, CancellationToken cancellation = default);
}
