namespace AccountService.API.Controllers;
using AccountService.BLL.DTOs.Responses;
using AccountService.BLL.UseCases.Account.Commands.Create;
using AccountService.BLL.UseCases.Account.Commands.Delete;
using AccountService.BLL.UseCases.Account.Commands.Transfer;
using AccountService.BLL.UseCases.Account.Commands.Update;
using AccountService.BLL.UseCases.Account.Queries.GetAllByUserId;
using AccountService.BLL.UseCases.Account.Queries.GetAllTransfersById;
using AccountService.BLL.UseCases.Account.Queries.GetById;
using MediatR;
using Microsoft.AspNetCore.Mvc;


[Route("api/[controller]")]
[ApiController]
public class AccountController(IMediator mediator) : ControllerBase
{
    //[Authorize]
    [HttpPost]
    public async Task<IActionResult> CreateAccount(CancellationToken cancellationToken = default)
    {
        //var userId = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value ?? throw new UnauthorizedAccessException("User is unauthorized");
        //var convertedId = int.Parse(userId);
        var convertedId = 0;
        var command = new CreateAccountCommand { UserId = convertedId };
        await mediator.Send(command, cancellationToken);
        return Ok();
    }

    [HttpGet("{accountId}")]
    public async Task<ActionResult<AccountResponse>> GetAccountById(Guid accountId, CancellationToken cancellationToken = default)
    {
        var query = new GetAccountByIdQuery(accountId);
        var account = await mediator.Send(query, cancellationToken);
        return Ok(account);
    }

    [HttpGet("all-{userId}")]
    public async Task<ActionResult<IEnumerable<AccountResponse>>> GetAllAccountsByUserId(int userId, CancellationToken cancellationToken = default)
    {
        //var userIdForCheck = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value ?? throw new UnauthorizedAccessException("User is unauthorized");
        //var convertedId = int.Parse(userIdForCheck);
        //convertedId = 0;
        //if (convertedId != userId)
        //{
        //    throw new BadRequestException("Client can get only his own data");
        //}
        var query = new GetAllAccountsByUserIdQuery(userId);
        var accounts = await mediator.Send(query, cancellationToken);
        return Ok(accounts);
    }

    //[Authorize]
    [HttpPatch]
    public async Task<IActionResult> UpdateAccount(UpdateAccountCommand command, CancellationToken cancellationToken = default)
    {
        await mediator.Send(command, cancellationToken);
        return Ok();
    }

    [HttpDelete("{accountId}")]
    public async Task<IActionResult> DeleteAccount(Guid accountId, CancellationToken cancellationToken = default)
    {
        var command = new DeleteAccountCommand { Id = accountId };
        await mediator.Send(command, cancellationToken);
        return Ok();
    }

    [HttpPost("transfer")]
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
