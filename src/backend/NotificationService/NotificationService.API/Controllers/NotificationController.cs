namespace NotificationService.API.Controllers;

using Microsoft.AspNetCore.Mvc;
using NotificationService.Application.Contracts.UseCases.Notifications;
using NotificationService.Application.DTOs.Requests;
using NotificationService.Application.DTOs.Responses;

[ApiController]
[Route("api/[controller]")]
public class NotificationController(
        ICreateUseCase createNotificationUseCase,
        IGetAllUseCase getAllNotificationsUseCase,
        IGetUnreadUseCase getUnreadNotificationsUseCase,
        IGetUnreadCountUseCase getUnreadNotificationCountUseCase) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateNotificationRequest dto, CancellationToken cancellation = default)
    {
        await createNotificationUseCase.ExecuteAsync(dto, cancellation);
        return Ok();
    }

    [HttpGet("/all/{userId}")]
    public async Task<List<NotificationResponse>> GetAll(int userId, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = -1, CancellationToken cancellation = default)
    {
        var result = await getAllNotificationsUseCase.ExecuteAsync(userId, pageNumber, pageSize, cancellation);
        return result;
    }

    [HttpGet("/unread/{userId}")]
    public async Task<List<NotificationResponse>> GetUnread(int userId, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = -1, CancellationToken cancellation = default)
    {
        var result = await getUnreadNotificationsUseCase.ExecuteAsync(userId, pageNumber, pageSize, cancellation);
        return result;
    }

    [HttpGet("unread/count/{userId}")]
    public async Task<ActionResult<int>> GetUnreadCount(int userId, CancellationToken cancellation = default)
    {
        var result = await getUnreadNotificationCountUseCase.ExecuteAsync(userId, cancellation);
        return Ok(result);
    }
}