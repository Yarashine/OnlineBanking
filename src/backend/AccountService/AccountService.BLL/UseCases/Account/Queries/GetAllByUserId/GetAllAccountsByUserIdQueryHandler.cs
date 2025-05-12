using AccountService.BLL.DTOs.Responses;
using AccountService.DAL.Contracts.Repositories;
using AutoMapper;
using MediatR;

namespace AccountService.BLL.UseCases.Account.Queries.GetAllByUserId;

public class GetAllAccountsByUserIdQueryHandler(
    IAccountRepository accountRepository,
    IMapper autoMapper) : IRequestHandler<GetAllAccountsByUserIdQuery, IEnumerable<AccountResponse>>
{
    public async Task<IEnumerable<AccountResponse>> Handle(GetAllAccountsByUserIdQuery request, CancellationToken cancellationToken = default)
    {
        var accounts = await accountRepository.GetAllByUserIdAsync(request.UserId, cancellationToken);
        return autoMapper.Map<IEnumerable<AccountResponse>>(accounts);
    }
}