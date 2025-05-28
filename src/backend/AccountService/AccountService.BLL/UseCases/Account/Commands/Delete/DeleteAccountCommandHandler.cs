using AccountService.DAL.Contracts.Repositories;
using AccountService.DAL.Exceptions;
using MediatR;

namespace AccountService.BLL.UseCases.Account.Commands.Delete;

public class DeleteAccountCommandHandler(
    IUnitOfWork unitOfWork) : IRequestHandler<DeleteAccountCommand>
{
    public async Task Handle(DeleteAccountCommand request, CancellationToken cancellationToken = default)
    {
        var accountForCheck = await unitOfWork.AccountRepository.GetByIdAsync(request.Id, cancellationToken) ?? throw new NotFoundException("Account");
        await unitOfWork.AccountRepository.DeleteAsync(request.Id, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}