using MediatR;

namespace AccountService.BLL.UseCases.Account.Commands.Delete;

public class DeleteAccountCommand : IRequest
{
    public Guid Id { get; set; }
}