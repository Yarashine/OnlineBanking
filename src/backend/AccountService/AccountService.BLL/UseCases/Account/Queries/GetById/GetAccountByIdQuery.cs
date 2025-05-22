using AccountService.BLL.DTOs.Responses;
using MediatR;

namespace AccountService.BLL.UseCases.Account.Queries.GetById;

public record GetAccountByIdQuery(Guid Id) : IRequest<AccountResponse>;