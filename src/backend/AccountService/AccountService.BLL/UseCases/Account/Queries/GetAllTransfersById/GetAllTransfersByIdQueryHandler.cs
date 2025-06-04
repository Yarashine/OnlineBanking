using AccountService.BLL.DTOs.Responses;
using AccountService.DAL.Configs;
using AccountService.DAL.Contracts.Repositories;
using AccountService.DAL.Exceptions;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Options;

namespace AccountService.BLL.UseCases.Account.Queries.GetAllTransfersById;

public class GetAllTransfersByIdQueryHandler(
    IUnitOfWork unitOfWork,
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

        pageSize = 1000;

        var accountForCheck = await unitOfWork.AccountRepository.GetByIdAsync(request.Id, cancellationToken) 
            ?? throw new NotFoundException("Account");

        var transfers = await unitOfWork.TransferRepository.GetAllByIdAsync(request.Id, pageNumber, pageSize, cancellationToken);

        return autoMapper.Map<IEnumerable<TransferResponse>>(transfers);
    }
}