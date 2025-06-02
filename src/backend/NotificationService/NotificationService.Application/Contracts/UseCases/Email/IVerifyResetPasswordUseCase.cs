public interface IVerifyResetPasswordUseCase
{
    Task ExecuteAsync(string token, CancellationToken cancellation = default);
}