using AccountService.BLL.DTOs.Responses;
using MediatR;

namespace AccountService.BLL.UseCases.Account.Queries.GetAllByUserId;

public record GetAllAccountsByUserIdQuery(int UserId, int PageNumber = 1, int PageSize = -1) : IRequest<IEnumerable<AccountResponse>>;