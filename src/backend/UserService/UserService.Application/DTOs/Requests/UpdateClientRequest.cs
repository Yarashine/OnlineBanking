namespace UserService.Application.DTOs.Requests;

public class UpdateClientRequest
{
    public string Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Patronymic { get; set; }
    public string PhoneNumber { get; set; }
    public string PassportIdentifier { get; set; }
    public int UserId { get; set; }
}
