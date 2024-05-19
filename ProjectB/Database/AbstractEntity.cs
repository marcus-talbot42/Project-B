namespace ProjectB.Models;

public abstract class AbstractEntity : IEntity
{
    public long Id { get; }

    public long GetId() => Id;
}