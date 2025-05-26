namespace AccountService.BLL.DTOs.Responses;

public class AccountResponse
{
    public Guid Id { get; set; }
    public int UserId { get; set; }
    public decimal Balance { get; set; }
    public DateTime CreatedAt { get; set; }
}