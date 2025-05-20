using AccountService.BLL.DTOs.Responses;
using AccountService.DAL.Configs;
using AccountService.DAL.Contracts.Repositories;
using AccountService.DAL.Exceptions;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Options;

namespace AccountService.BLL.UseCases.Account.Queries.GetAllTransfersById;

public class GetAllTransfersByIdQueryHandler(
    IAccountRepository accountRepository,
    ITransferRepository transferRepository,
    IOptions<PaginationSettings> paginationOptions,
    IMapper autoMapper) : IRequestHandler<GetAllTransfersByIdQuery, IEnumerable<TransferResponse>>
{

    private readonly PaginationSettings paginationSettings = paginationOptions.Value;
    public async Task<IEnumerable<TransferResponse>> Handle(GetAllTransfersByIdQuery request, CancellationToken cancellationToken = default)
    {
        var pageSize = request.PageSize;

        if (pageSize <= 0)
        {
            pageSize = paginationSettings.DefaultPageSize;
        }

        var pageNumber = request.PageNumber;

        if (pageNumber <= 0)
        {
            pageNumber = 1;
        }

        var accountForCheck = await accountRepository.GetByIdAsync(request.Id, cancellationToken) 
            ?? throw new NotFoundException("Account");

        var transfers = await transferRepository.GetAllByIdAsync(request.Id, pageNumber, pageSize, cancellationToken);

        return autoMapper.Map<IEnumerable<TransferResponse>>(transfers);
    }
}