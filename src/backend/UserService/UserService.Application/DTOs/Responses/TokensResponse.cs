namespace UserService.Application.DTOs.Responses;

public class TokensResponse
{
    public string RefreshToken { get; set; }
    public string AccessToken { get; set; }
}
