namespace UserService.Application.DTOs.Requests;

public class SignInRequest
{
    public string Login { get; set; }
    public string Password { get; set; }
    public string DeviceId { get; set; }
}
