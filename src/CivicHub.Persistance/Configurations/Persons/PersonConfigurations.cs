using CivicHub.Domain.Persons;
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

        builder.OwnsMany(x => x.PhoneNumbers, phone =>
        {
            phone.Property(p => p.Number)
                .HasMaxLength(15)
                .IsRequired();
        });

        builder.HasOne(x => x.Location)
            .WithMany();

        builder.HasIndex(x => new { x.FirstName, x.LastName, x.PersonalNumber });
        builder.HasIndex(x => new { x.FirstName, x.LastName, x.PersonalNumber, x.Gender, x.BirthDate });
    }
}