namespace AccountService.DAL.Exceptions;

public class NotFoundException(string entity) : ApplicationException($"{entity} not found.")
{
}