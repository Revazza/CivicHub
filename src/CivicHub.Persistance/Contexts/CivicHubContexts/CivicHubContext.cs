using CivicHub.Domain.Cities;
using CivicHub.Domain.Persons;
using CivicHub.Domain.Persons.Entities.PersonConnections;
using Microsoft.EntityFrameworkCore;

namespace CivicHub.Persistance.Contexts.CivicHubContexts;

public class CivicHubContext(DbContextOptions<CivicHubContext> options) : DbContext(options)
{
    public const string SectionName = nameof(CivicHubContext);
    public const string SchemaName = "CivicHub";
    public DbSet<Person> Persons { get; set; }
    public DbSet<PersonConnection> PersonConnections { get; set; }
    public DbSet<City> Cities { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(SchemaName);
        //Apply all the configurations defined in Configurations folder
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CivicHubContext).Assembly);

        SeedData(modelBuilder);
    }

    private static void SeedData(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<City>().HasData(
            new City { Name = "Tbilisi", Code = "TB" },
            new City { Name = "Batumi", Code = "BT" }
        );
    }
}