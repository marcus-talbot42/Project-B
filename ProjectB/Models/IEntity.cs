namespace ProjectB.Models;

/// <summary>
/// This interface should be implemented by any type that will be saved to files, except for enums. It exposes the
/// GetId-method, which returns the ID of the object is in called on.
/// </summary>
/// 
/// <typeparam name="TId">The type of the ID of the entity.</typeparam>
public interface IEntity
{
    long GetId();
}