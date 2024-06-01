using System.Data;

namespace ProjectB.Exceptions;

/// <summary>
/// Exception used to show that an entity with the specified ID (primary key) already exists.
/// </summary>
/// <param name="message">The message that will be shown when such an exception is thrown.</param>
public class PrimaryKeyConstraintException(string message) : ConstraintException(message)
{

}