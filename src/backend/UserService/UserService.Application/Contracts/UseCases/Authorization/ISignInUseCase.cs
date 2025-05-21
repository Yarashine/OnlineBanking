using System.Threading;
using System.Threading.Tasks;
using UserService.Application.DTOs.Requests;
using UserService.Application.DTOs.Responses;

namespace UserService.Application.Contracts.UseCases.Authorization;

public interface ISignInUseCase
{
    Task<TokensResponse> ExecuteAsync(SignInRequest request, CancellationToken cancellationToken = default);
}
