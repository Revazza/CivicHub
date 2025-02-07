using CivicHub.Domain.Persons;
using CivicHub.Domain.Persons.ValueObjects.PhoneNumbers;
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
            .IsRequired();

        builder.Property(x => x.LastName)
            .HasAnnotation(MinLengthAnnotation, PersonConstraints.LastNameMinLength)
            .HasMaxLength(PersonConstraints.LastNameMaxLength)
            .IsRequired();

        builder.Property(x => x.PersonalNumber)
            .HasMaxLength(PersonConstraints.PersonalNumberLength)
            .IsRequired();

        builder.HasIndex(x => x.PersonalNumber).IsUnique();

        builder.OwnsMany(x => x.PhoneNumbers, phone =>
        {
            phone.Property(p => p.CountryCode)
                .HasMaxLength(PhoneNumberConstraints.MaxCountryCodeLength)
                .IsRequired();
            
            phone.Property(p => p.AreaCode)
                .HasMaxLength(PhoneNumberConstraints.MaxAreaCodeLength)
                .IsRequired();
            
            phone.Property(p => p.Number)
                .HasMaxLength(PhoneNumberConstraints.MaxNumberLength)
                .IsRequired();
        });

        builder.HasOne(x => x.City)
            .WithMany()
            .HasForeignKey(x => x.CityCode);

        builder.HasIndex(x => new { x.FirstName, x.LastName, x.PersonalNumber });
        builder.HasIndex(x => new { x.FirstName, x.LastName, x.PersonalNumber, x.Gender, x.BirthDate });
    }
}