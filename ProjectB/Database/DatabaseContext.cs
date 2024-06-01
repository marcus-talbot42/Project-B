using Microsoft.EntityFrameworkCore;
using ProjectB.Models;
using System.Text.Json;

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

    public override int SaveChanges()
    {
        WriteToJson(Employees);
        WriteToJson(Guests);
        WriteToJson(Tours);
        WriteToJson(Translations);

        return base.SaveChanges();
    }

    private IEnumerable<T> ReadFromJson<T>() where T : AbstractEntity
    {
        return JsonSerializer.Deserialize<IEnumerable<T>>(
            File.ReadAllText($"Json/{typeof(T).Name}.json"))!;
    }

    private void WriteToJson<T>(IEnumerable<T> entities) where T : AbstractEntity
    {
        File.WriteAllText($"Json/{typeof(T).Name}.json", JsonSerializer.Serialize(entities, new JsonSerializerOptions { WriteIndented = true }));
    }

    public DbSet<T>? GetRelevantDbSet<T>() where T : AbstractEntity
    {
        if (typeof(T) == typeof(Employee))
        {
            return Employees as DbSet<T>;
        }

        if (typeof(T) == typeof(Guest))
        {
            return Guests as DbSet<T>;
        }

        if (typeof(T) == typeof(Tour))
        {
            return Tours as DbSet<T>;
        }

        if (typeof(T) == typeof(Translation))
        {
            return Translations as DbSet<T>;
        }

        throw new NullReferenceException($"Could not load DbSet<{typeof(T).Name}>...");
    }
}