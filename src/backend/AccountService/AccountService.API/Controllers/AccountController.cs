namespace AccountService.API.Controllers;
using AccountService.BLL.DTOs.Responses;
using AccountService.BLL.UseCases.Account.Commands.Create;
using AccountService.BLL.UseCases.Account.Commands.Delete;
using AccountService.BLL.UseCases.Account.Commands.Update;
using AccountService.BLL.UseCases.Account.Queries.GetAllByUserId;
using AccountService.BLL.UseCases.Account.Queries.GetById;
using MediatR;
using Microsoft.AspNetCore.Mvc;


[Route("api/account")]
[ApiController]
public class AccountController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateAccount(int userId, CancellationToken cancellationToken = default)
    {
        Console.WriteLine("create controller");
        var command = new CreateAccountCommand { UserId = userId };
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
        var query = new GetAllAccountsByUserIdQuery(userId);
        var accounts = await mediator.Send(query, cancellationToken);

        return Ok(accounts);
    }


    [HttpGet("all")]
    public async Task<ActionResult<IEnumerable<AccountResponse>>> GetAllAccounts(CancellationToken cancellationToken = default)
    {
        var query = new GetAllAccountsQuery();
        var accounts = await mediator.Send(query, cancellationToken);

        return Ok(accounts);
    }

    [HttpPut]
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
}
