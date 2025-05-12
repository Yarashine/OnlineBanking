namespace AccountService.BLL.DTOs.Responses;

public class TransferResponse
{
    public Guid Id { get; set; }
    public Guid SenderAccountId { get; set; }
    public Guid ReceiverAccountId { get; set; }
    public decimal Amount { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}