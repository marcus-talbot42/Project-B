namespace ProjectB.Models;

/// <summary>
/// This interface should be implemented by any type that will be saved to files, except for enums. It exposes the
/// GetId-method, which returns the ID of the object is in called on.
/// </summary>
/// 
/// <typeparam name="TId">The type of the ID of the entity.</typeparam>
public interface IEntity<TId>
{
    /// <summary>
    /// Returns the ID of the object it is called on. Mainly used to determine whether an ID is already taken, resulting
    /// in a PrimaryKeyConstraintException when attempting to create a new entity with that ID, or to determine which
    /// object should be updated.
    /// </summary>
    /// 
    /// <returns>The ID of the object it is called on.</returns>
    /// <typeparam name="TId">The type of the ID.</typeparam>
    /// 
    TId GetId();
}