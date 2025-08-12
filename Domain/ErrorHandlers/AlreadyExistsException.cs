namespace Domain.ErrorHandlers;

public class AlreadyExistsException(string message) : Exception(message)
{
    
}