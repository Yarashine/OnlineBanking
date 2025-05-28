namespace AccountService.DAL.Exceptions;

public class BadRequestException(string message) : ApplicationException(message)
{
}