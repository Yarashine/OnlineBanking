using MediatR;

namespace AccountService.BLL.UseCases.Account.Commands.Create;

public class CreateAccountCommand : IRequest
{
    public int UserId { get; set; }
}