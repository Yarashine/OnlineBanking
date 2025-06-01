using Grpc.Core;
using UserService;
using UserService.Application.Contracts.UseCases.Authorization;

namespace UserService.API.Services.GRPC;

public class UserGrpcService(
    IGenerateTokenForEmailConfirmationUseCase generateTokenUseCase,
    IGenerateTokenForResetPassword generateTokenForResetPassword) : UserService.UserServiceBase
{
    public override async Task<GenerateTokenResponse> GenerateEmailConfirmationToken(
    GenerateTokenRequest request, ServerCallContext context)
    {
        var token = await generateTokenUseCase.ExecuteAsync(request.Email, context.CancellationToken);
        return new GenerateTokenResponse { Token = token };
    }

    public override async Task<GenerateTokenResponse> GeneratePasswordResetToken(
        GenerateTokenRequest request, ServerCallContext context)
    {
        var token = await generateTokenForResetPassword.ExecuteAsync(request.Email, context.CancellationToken);
        return new GenerateTokenResponse { Token = token };
    }
}
