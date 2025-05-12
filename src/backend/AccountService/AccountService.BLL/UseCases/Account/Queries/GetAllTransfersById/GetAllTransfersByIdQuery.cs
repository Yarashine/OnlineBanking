using AccountService.BLL.DTOs.Responses;
using MediatR;

namespace AccountService.BLL.UseCases.Account.Queries.GetAllTransfersById;

public record GetAllTransfersByIdQuery(Guid Id) : IRequest<IEnumerable<TransferResponse>>;