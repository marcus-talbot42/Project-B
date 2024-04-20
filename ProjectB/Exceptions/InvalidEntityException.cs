using System.Data;

namespace ProjectB.Exceptions;

public class InvalidEntityException(string message) : ConstraintException(message)
{
    
}