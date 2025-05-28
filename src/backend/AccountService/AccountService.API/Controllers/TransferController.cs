namespace AccountService.API.Controllers;
using AccountService.BLL.DTOs.Responses;
using AccountService.BLL.UseCases.Account.Commands.Transfer;
using AccountService.BLL.UseCases.Account.Queries.GetAllTransfersById;
using MediatR;
using Microsoft.AspNetCore.Mvc;


[Route("api/transfer")]
[ApiController]
public class TransferController(IMediator mediator) : ControllerBase
{

    [HttpPost]
    public async Task<IActionResult> Transfer(TransferAccountCommand command, CancellationToken cancellationToken = default)
    {
        await mediator.Send(command, cancellationToken);

        return Ok();
    }

    [HttpGet("all-transfers-{accountId}")]
    public async Task<ActionResult<IEnumerable<TransferResponse>>> GetAllAccountsByUserId(Guid accountId, CancellationToken cancellationToken = default)
    {
        var query = new GetAllTransfersByIdQuery(accountId);
        var accounts = await mediator.Send(query, cancellationToken);

        return Ok(accounts);
    }
}
