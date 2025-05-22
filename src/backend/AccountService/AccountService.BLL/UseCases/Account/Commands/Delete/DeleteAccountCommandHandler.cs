using AccountService.DAL.Contracts.Repositories;
using AccountService.DAL.Exceptions;
using MediatR;

namespace AccountService.BLL.UseCases.Account.Commands.Delete;

public class DeleteAccountCommandHandler(IAccountRepository accountRepository) : IRequestHandler<DeleteAccountCommand>
{
    public async Task Handle(DeleteAccountCommand request, CancellationToken cancellationToken = default)
    {
        var accountForCheck = await accountRepository.GetByIdAsync(request.Id, cancellationToken) ?? throw new NotFoundException("Account");
        await accountRepository.DeleteAsync(request.Id, cancellationToken);
    }
}