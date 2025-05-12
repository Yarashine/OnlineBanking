using AccountService.BLL.DTOs.Responses;
using MediatR;

namespace AccountService.BLL.UseCases.Account.Queries.GetAllByUserId;

public record GetAllAccountsByUserIdQuery(int UserId) : IRequest<IEnumerable<AccountResponse>>;