using Microsoft.EntityFrameworkCore;
using ProjectB.Models;

namespace ProjectB.Database
{
    public interface IDatabaseContext
    {
        DbSet<Employee> Employees { get; set; }
        DbSet<Guest> Guests { get; set; }
        DbSet<Tour> Tours { get; set; }
        DbSet<Translation> Translations { get; set; }

        int SaveChanges();

        DbSet<T>? GetRelevantDbSet<T>() where T : AbstractEntity;
    }
}