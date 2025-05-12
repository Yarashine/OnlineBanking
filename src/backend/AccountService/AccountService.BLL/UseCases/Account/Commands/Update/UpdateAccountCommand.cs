using MediatR;

namespace AccountService.BLL.UseCases.Account.Commands.Update;

public class UpdateAccountCommand : IRequest
{
    public Guid Id { get; set; }
    public int UserId { get; set; }
    public decimal Balance { get; set; }
}