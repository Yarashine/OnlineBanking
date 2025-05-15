using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserService.Application.Contracts.UseCases.Clients;
using UserService.Application.DTOs.Requests;
using UserService.Application.DTOs.Responses;

namespace UserService.API.Controllers;

[ApiController]
[Route("api/client")]
public class ClientController(
    ICreateUseCase createClient,
    IUpdateUseCase updateClient,
    IDeleteUseCase deleteClient,
    IGetAllUseCase getAllClients,
    IGetByIdUseCase getClientById,
    IGetByUserIdUseCase getClientByUserId) : ControllerBase
{
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Create(CreateClientRequest request, CancellationToken cancellation = default)
    {
        var userId = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value ?? throw new UnauthorizedAccessException("User is unauthorized");
        var convertedId = int.Parse(userId);
        await createClient.ExecuteAsync(request, convertedId, cancellation);
        return Ok();
    }

    [Authorize]
    [HttpPut]
    public async Task<IActionResult> Update(UpdateClientRequest request, CancellationToken cancellation = default)
    {
        await updateClient.ExecuteAsync(request, cancellation);
        return Ok();
    }

    [Authorize]
    [HttpDelete]
    public async Task<IActionResult> Delete(string id, CancellationToken cancellation = default)
    {
        await deleteClient.ExecuteAsync(id, cancellation);
        return Ok();
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetById(string id, CancellationToken cancellation = default)
    {
        var client = await getClientById.ExecuteAsync(id, cancellation);
        return Ok(client);
    }

    [Authorize]
    [HttpGet("user-id")]
    public async Task<ActionResult<ClientResponse>> GetByUserId(int id, CancellationToken cancellation = default)
    {
        var response = await getClientByUserId.ExecuteAsync(id, cancellation);
        return Ok(response);
    }

    [HttpGet("all")]
    public async Task<ActionResult<IEnumerable<ClientResponse>>> GetAll(CancellationToken cancellation = default)
    {
        var clients = await getAllClients.ExecuteAsync(cancellation);
        return Ok(clients);
    }
}
