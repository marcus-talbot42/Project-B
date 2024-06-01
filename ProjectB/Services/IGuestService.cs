using ProjectB.Models;

namespace ProjectB.Services;

public interface IGuestService : IService<Guest>
{
    Guest FindValidGuestById(string username);
}