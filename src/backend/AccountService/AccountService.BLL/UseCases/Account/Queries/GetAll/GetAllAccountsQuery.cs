using AccountService.BLL.DTOs.Responses;
using MediatR;

namespace AccountService.BLL.UseCases.Account.Queries.GetAllByUserId;

public record GetAllAccountsQuery(int PageNumber = 1, int PageSize = -1) : IRequest<IEnumerable<AccountResponse>>;