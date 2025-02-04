using CivicHub.Domain.Locations;
using CivicHub.Domain.Persons;
using CivicHub.Domain.Persons.Entities.PersonConnections;
using Microsoft.EntityFrameworkCore;

namespace CivicHub.Persistance.Contexts.CivicHubContexts;

public class CivicHubContext(DbContextOptions<CivicHubContext> options) : DbContext(options)
{
    public DbSet<Person> Persons { get; set; }
    public DbSet<PersonConnection> PersonConnections { get; set; }
    public DbSet<Location> Locations { get; set; }
}