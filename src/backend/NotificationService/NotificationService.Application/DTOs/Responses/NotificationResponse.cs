namespace NotificationService.Application.DTOs.Responses;

public class NotificationResponse
{
    public Guid Id { get; set; }
    public string Message { get; set; }
    public bool IsRead { get; set; }
    public DateTime CreatedAt { get; set; }
}