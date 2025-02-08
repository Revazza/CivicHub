using CivicHub.Domain.Persons.Entities.PersonConnections;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CivicHub.Persistance.Configurations.PersonConnections;

public class PersonConnectionsConfigurations : IEntityTypeConfiguration<PersonConnection>
{
    public void Configure(EntityTypeBuilder<PersonConnection> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();

        builder.HasOne(x => x.Person)
            .WithMany(x => x.Connections)
            .HasForeignKey(x => x.PersonId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasOne(x => x.ConnectedPerson)
            .WithMany(x => x.ConnectedTo)
            .HasForeignKey(x => x.ConnectedPersonId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasIndex(x => new { x.PersonId, x.ConnectedPersonId })
            .IsUnique();
    }
}