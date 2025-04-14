using System.Threading;
using System.Threading.Tasks;
using UserService.Application.DTOs.Requests;
using UserService.Application.DTOs.Responses;

namespace UserService.Application.Contracts.UseCases.Authorization;

public interface ISignUpUseCase
{
    Task<TokensResponse> ExecuteAsync(SignUpRequest request, CancellationToken cancellationToken = default);
}
