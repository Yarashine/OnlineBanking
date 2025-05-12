using AccountService.BLL.DTOs.Responses;
using AccountService.DAL.Contracts.Repositories;
using AccountService.DAL.Exceptions;
using AutoMapper;
using MediatR;

namespace AccountService.BLL.UseCases.Account.Queries.GetAllTransfersById;

public class GetAllTransfersByIdQueryHandler(
    IAccountRepository accountRepository,
    IMapper autoMapper) : IRequestHandler<GetAllTransfersByIdQuery, IEnumerable<TransferResponse>>
{
    public async Task<IEnumerable<TransferResponse>> Handle(GetAllTransfersByIdQuery request, CancellationToken cancellationToken = default)
    {
        var accountForCheck = await accountRepository.GetByIdAsync(request.Id, cancellationToken) ?? throw new NotFoundException("Account");
        var transfers = await accountRepository.GetAllTransfersByIdAsync(request.Id, cancellationToken);
        return autoMapper.Map<IEnumerable<TransferResponse>>(transfers);
    }
}