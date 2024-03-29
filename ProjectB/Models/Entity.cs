namespace ProjectB.Models;

public interface Entity<TId>
{
    TId GetId();
}