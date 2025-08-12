namespace Domain.ErrorHandlers;

public class NotFoundException(string message) : Exception(message);