namespace UserService.Domain.Exceptions;

public class AlreadyExistsException(string message) : ApplicationException(message)
{
}