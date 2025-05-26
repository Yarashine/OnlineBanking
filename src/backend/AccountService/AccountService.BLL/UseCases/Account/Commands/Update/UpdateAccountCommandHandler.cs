using AccountService.DAL.Contracts.Repositories;
using AccountService.DAL.Exceptions;
using AutoMapper;
using MediatR;

namespace AccountService.BLL.UseCases.Account.Commands.Update;

public class UpdateAccountCommandHandler(
    IUnitOfWork unitOfWork,
    IMapper autoMapper) : IRequestHandler<UpdateAccountCommand>
{
    public async Task Handle(UpdateAccountCommand request, CancellationToken cancellationToken = default)
    {
        var accountForCheck = await unitOfWork.AccountRepository.GetByIdAsync(request.Id, cancellationToken) ?? throw new NotFoundException("Account");
        var account = autoMapper.Map<DAL.Entities.Account>(request);
        unitOfWork.AccountRepository.Update(account, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}