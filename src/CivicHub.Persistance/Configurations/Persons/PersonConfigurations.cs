using CivicHub.Domain.Cities;
using CivicHub.Domain.Persons;
using CivicHub.Domain.Persons.ValueObjects.PhoneNumbers;
using CivicHub.Persistance.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CivicHub.Persistance.Configurations.Persons;

public class PersonConfigurations : IEntityTypeConfiguration<Person>
{
    private const string MinLengthAnnotation = "MinLength";

    public void Configure(EntityTypeBuilder<Person> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();

        builder.Property(x => x.FirstName)
            .HasAnnotation(MinLengthAnnotation, PersonConstraints.FirstNameMinLength)
            .HasMaxLength(PersonConstraints.FirstNameMaxLength)
            .UseCollation(DatabaseCollations.CaseInsensitiveCollation)
            .IsRequired();

        builder.Property(x => x.LastName)
            .HasAnnotation(MinLengthAnnotation, PersonConstraints.LastNameMinLength)
            .HasMaxLength(PersonConstraints.LastNameMaxLength)
            .UseCollation(DatabaseCollations.CaseInsensitiveCollation)
            .IsRequired();

        builder.Property(x => x.PersonalNumber)
            .HasMaxLength(PersonConstraints.PersonalNumberLength)
            .IsRequired();

        builder.Property(x => x.CityCode)
            .HasMaxLength(CityConstraints.MaxCityCodeLength)
            .UseCollation(DatabaseCollations.CaseInsensitiveCollation)
            .IsRequired();

        builder.HasIndex(x => x.PersonalNumber).IsUnique();

        builder.OwnsMany(x => x.PhoneNumbers, phone =>
        {
            phone.Property(p => p.Number)
                .HasMaxLength(PhoneNumberConstraints.MaxNumberLength)
                .IsRequired();
        });

        builder.HasOne(x => x.City)
            .WithMany()
            .HasForeignKey(x => x.CityCode);

        builder.Property(x => x.PictureFullPath).HasMaxLength(PersonConstraints.PicturePathMaxLength);

        builder.HasIndex(x => new { x.FirstName, x.LastName, x.PersonalNumber });
        builder.HasIndex(x => new { x.FirstName, x.LastName, x.PersonalNumber, x.Gender, x.BirthDate });
    }
}