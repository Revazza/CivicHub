using CivicHub.Domain.Cities;
using CivicHub.Persistance.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CivicHub.Persistance.Configurations.Cities;

public class CityConfigurations : IEntityTypeConfiguration<City>
{
    public void Configure(EntityTypeBuilder<City> builder)
    {
        builder.HasKey(x => x.Code);
        builder.Property(x => x.Code)
            .ValueGeneratedNever();

        builder.Property(x => x.Code)
            .HasMaxLength(CityConstraints.MaxCityCodeLength)
            .UseCollation(DatabaseCollations.CaseInsensitiveCollation)
            .IsRequired();
        
        builder.Property(x => x.Name)
            .HasMaxLength(CityConstraints.MaxCityNameLength)
            .UseCollation(DatabaseCollations.CaseInsensitiveCollation)
            .IsRequired();
    }
}