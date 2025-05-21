namespace UserService.Application.DTOs.Requests;

public class RefreshRequest
{
    public string RefreshToken { get; set; }
    public string DeviceId { get; set; }
    public string UserId { get; set; }
}
