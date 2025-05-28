namespace UserService.Domain.Exceptions;

public class NotFoundException(string message) : ApplicationException(message)
{
}