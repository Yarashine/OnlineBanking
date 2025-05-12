using AccountService.DAL.Contracts.Repositories;
using AccountService.DAL.Exceptions;
using AutoMapper;
using MediatR;

namespace AccountService.BLL.UseCases.Account.Commands.Transfer;

public class TransferAccountCommandHandler(
    IAccountRepository accountRepository,
    IMapper autoMapper) : IRequestHandler<TransferAccountCommand>
{
    public async Task Handle(TransferAccountCommand request, CancellationToken cancellationToken = default)
    {
        var accountForCheckSender = await accountRepository.GetByIdAsync(request.SenderAccountId, cancellationToken) ?? throw new NotFoundException("Account");
        if (accountForCheckSender.Balance < request.Amount)
        {
            throw new BadRequestException("You cannot transfer less funds than you have on your account");
        }

        var accountForCheckReceiver = await accountRepository.GetByIdAsync(request.ReceiverAccountId, cancellationToken) ?? throw new NotFoundException("Account");
        var transfer = autoMapper.Map<DAL.Entities.Transfer>(request);
        await accountRepository.CreateTransferAsync(transfer, cancellationToken);
    }
}