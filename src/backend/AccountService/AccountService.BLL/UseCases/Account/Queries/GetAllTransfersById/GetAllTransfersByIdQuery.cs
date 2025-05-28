using AccountService.BLL.DTOs.Responses;
using MediatR;

namespace AccountService.BLL.UseCases.Account.Queries.GetAllTransfersById;

public record GetAllTransfersByIdQuery(Guid Id, int PageNumber = 1, int PageSize = -1) : IRequest<IEnumerable<TransferResponse>>;