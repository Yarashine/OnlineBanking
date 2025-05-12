namespace AccountService.BLL.DTOs.Responses;

public class AccountResponse
{
    public Guid Id { get; set; }
    public int UserId { get; set; }
    public decimal Balance { get; set; }
    public string Currency { get; set; }
    public string Status { get; set; }
    public DateTime CreatedAt { get; set; }
}