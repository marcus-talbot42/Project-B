namespace ProjectB.Models;

public abstract class AbstractEntity : IEntity<long>
{
    public long Id { get; }

    public long GetId() => Id;
}