using AccountService.BLL.DTOs.Responses;
using AccountService.DAL.Configs;
using AccountService.DAL.Contracts.Repositories;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Options;

namespace AccountService.BLL.UseCases.Account.Queries.GetAllByUserId;

public class GetAllAccountsByUserIdQueryHandler(
    IAccountRepository accountRepository,
    IOptions<PaginationSettings> paginationOptions,
    IMapper autoMapper) : IRequestHandler<GetAllAccountsByUserIdQuery, IEnumerable<AccountResponse>>
{

    private readonly PaginationSettings paginationSettings = paginationOptions.Value;
    public async Task<IEnumerable<AccountResponse>> Handle(GetAllAccountsByUserIdQuery request, CancellationToken cancellationToken = default)
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

        var accounts = await accountRepository.GetAllByUserIdAsync(request.UserId, pageNumber, pageSize, cancellationToken);
        return autoMapper.Map<IEnumerable<AccountResponse>>(accounts);
    }
}