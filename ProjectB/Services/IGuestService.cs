using ProjectB.Exceptions;
using ProjectB.Models;
using ProjectB.Repositories;

namespace ProjectB.Services;

public interface IGuestService : IService<Guest>
{
    Guest FindValidGuestById(string username);
}