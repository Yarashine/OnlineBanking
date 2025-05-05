namespace NotificationService.Application.Contracts.UseCases;

public interface IUseCase<in TRequest, TResponse>
{
    Task<TResponse> ExecuteAsync(TRequest request, CancellationToken cancellation = default);
}