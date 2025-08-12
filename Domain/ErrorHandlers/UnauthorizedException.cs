namespace Domain.ErrorHandlers;

public class UnauthorizedException(string message) : Exception(message)
{
    
}