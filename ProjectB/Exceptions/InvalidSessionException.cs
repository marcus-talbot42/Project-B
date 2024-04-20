namespace ProjectB.Exceptions;

public class InvalidSessionException(string message) : AccessViolationException(message)
{
    
}