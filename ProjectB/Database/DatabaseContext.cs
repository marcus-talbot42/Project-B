using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using ProjectB.Models;

namespace ProjectB.Database;

public class DatabaseContext : DbContext
{
    public DatabaseContext(DbContextOptions options) : base(options)
    {
        LoadData();
    }

    public DbSet<AbstractEntity> AbstractEntities { get; set; }
    public DbSet<AbstractUser> AbstractUsers { get; set; }
    public DbSet<Employee> Employees { get; set; }
    public DbSet<Guest> Guests { get; set; }
    public DbSet<Tour> Tours { get; set; }
    public DbSet<Translation> Translations { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseInMemoryDatabase("DepotDB").EnableSensitiveDataLogging().EnableDetailedErrors();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AbstractEntity>().HasKey(ae => ae.Id);
    }

    private void LoadData()
    {
        Employees.AddRange(ReadFromJson<Employee>());
        Guests.AddRange(ReadFromJson<Guest>());
        Tours.AddRange(ReadFromJson<Tour>());
        Translations.AddRange(ReadFromJson<Translation>());
        SaveChanges();
    }

    private IEnumerable<TEntity> ReadFromJson<TEntity>() where TEntity : class, IEntity
    {
        return JsonSerializer.Deserialize<IEnumerable<TEntity>>(
            File.ReadAllText($".//../../../Database/{typeof(TEntity).Name}.json"))!;
    }

    public DbSet<TEntity>? GetRelevantDbSet<TEntity>()
        where TEntity : class, IEntity
    {
        if (typeof(TEntity) == typeof(Employee))
        {
            return Employees as DbSet<TEntity>;
        }

        if (typeof(TEntity) == typeof(Guest))
        {
            return Guests as DbSet<TEntity>;
        }

        if (typeof(TEntity) == typeof(Tour))
        {
            return Tours as DbSet<TEntity>;
        }
        
        if (typeof(TEntity) == typeof(Translation))
        {
            return Translations as DbSet<TEntity>;
        }

        throw new NullReferenceException($"Could not load DbSet<{typeof(TEntity).Name}>...");
    }
}