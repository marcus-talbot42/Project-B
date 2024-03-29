using System.Data;

namespace ProjectB.Exceptions;

public class PrimaryKeyConstraintException(string message) : ConstraintException(message)
{
    
}