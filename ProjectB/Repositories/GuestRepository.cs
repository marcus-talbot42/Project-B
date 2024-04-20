using ProjectB.Models;

namespace ProjectB.Repositories;

public class GuestRepository : AbstractRepository<Guest, string>
{

    private static readonly Lazy<GuestRepository> Lazy = new(() => new GuestRepository());
    public static GuestRepository Instance => Lazy.Value;

    private GuestRepository()
    {
    }
}