using System.Data;

namespace ProjectB.Exceptions;

public class NonNullConstraintException(string message) : ConstraintException(message)
{
}