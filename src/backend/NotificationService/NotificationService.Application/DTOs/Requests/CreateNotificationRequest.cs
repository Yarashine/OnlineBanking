namespace NotificationService.Application.DTOs.Requests;

public class CreateNotificationRequest
{
    public string Message { get; set; }
    public int UserId { get; set; }
}