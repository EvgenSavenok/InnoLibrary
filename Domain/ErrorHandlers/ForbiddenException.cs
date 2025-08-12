namespace Domain.ErrorHandlers;

public class ForbiddenException(string message) : Exception(message)
{
    
}