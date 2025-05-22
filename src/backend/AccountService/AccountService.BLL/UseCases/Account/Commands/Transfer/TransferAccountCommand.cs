using MediatR;

namespace AccountService.BLL.UseCases.Account.Commands.Transfer;

public class TransferAccountCommand : IRequest
{
    public Guid SenderAccountId { get; set; }
    public Guid ReceiverAccountId { get; set; }
    public decimal Amount { get; set; }
}