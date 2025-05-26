using AccountService.DAL.Contracts.Repositories;
using AccountService.DAL.Exceptions;
using AccountService.DAL.Repositories;
using AutoMapper;
using MediatR;

namespace AccountService.BLL.UseCases.Account.Commands.Transfer;

public class TransferAccountCommandHandler(
    IUnitOfWork unitOfWork,
    IMapper autoMapper) : IRequestHandler<TransferAccountCommand>
{
    public async Task Handle(TransferAccountCommand request, CancellationToken cancellationToken = default)
    {
        var transfer = autoMapper.Map<DAL.Entities.Transfer>(request);

        var sender = await unitOfWork.AccountRepository.GetByIdAsync(request.SenderAccountId, cancellationToken) ?? throw new NotFoundException("Sender account not found.");
        if (sender.Balance < request.Amount)
        {
            throw new BadRequestException("Insufficient funds in sender account.");
        }

        var receiver = await unitOfWork.AccountRepository.GetByIdAsync(request.ReceiverAccountId, cancellationToken) ?? throw new NotFoundException("Receiver account not found.");
        await unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            sender.Balance -= transfer.Amount;
            sender.UpdatedAt = DateTime.UtcNow;

            receiver.Balance += transfer.Amount;
            receiver.UpdatedAt = DateTime.UtcNow;

            unitOfWork.AccountRepository.Update(sender, cancellationToken);
            unitOfWork.AccountRepository.Update(receiver, cancellationToken);
            await unitOfWork.TransferRepository.CreateAsync(transfer, cancellationToken);

            await unitOfWork.SaveChangesAsync(cancellationToken);
            await unitOfWork.CommitTransactionAsync(cancellationToken);
        }
        catch
        {
            await unitOfWork.RollbackTransactionAsync(cancellationToken);
            throw;
        }
    }
}