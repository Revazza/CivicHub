 using AutoFixture;
using AutoFixture.Dsl;
using CivicHub.Application.Common.Extensions;
using CivicHub.Application.Features.Persons.Commands.AddPerson;
using CivicHub.Application.Features.Persons.Commands.Common.Dtos;
using CivicHub.Application.IntegrationTestsCommon;
using CivicHub.Application.IntegrationTestsCommon.Extensions;
using CivicHub.Domain.Cities;
using CivicHub.Domain.Persons.Enums;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace CivicHub.Application.IntegrationTests.Features.Persons.Commands.AddPerson;

public class AddPersonCommandHandlerTests : IntegrationTestBase
{
    private IPostprocessComposer<AddPersonCommand> _validCommandComposer;

    [SetUp]
    public async Task SetUp()
    {
        var phoneNumbers = Fixture
            .Build<PhoneNumberDto>()
            .With(x => x.Number, "5555")
            .Create();

        var city = Fixture.GenerateCity();

        _validCommandComposer = Fixture.Build<AddPersonCommand>()
            .With(x => x.FirstName, "DimItrI")
            .With(x => x.LastName, "DimItrIShviLi")
            .With(x => x.PersonalNumber, "12345678900")
            .With(x => x.CityCode, city.Code)
            .With(x => x.Gender, Gender.Male)
            .With(x => x.BirthDate, DateTime.UtcNow.AddYears(-20))
            .With(x => x.PhoneNumbers, [phoneNumbers]);

        await Context.Cities.AddAsync(city);
        await Context.SaveChangesAsync();
    }

    [Test]
    public async Task When_IsValidCommand_Then_PersonIsAddedCorrectly()
    {
        // Arrange
        var command = _validCommandComposer.Create();

        // Act
        var result = await Sender.Send(command);

        // Assert
        Assert.That(result.IsSuccess, Is.True);

        // Assert #2
        var person = await Context
            .Persons
            .Include(x => x.PhoneNumbers)
            .Include(x => x.City)
            .SingleOrDefaultAsync(person => person.PersonalNumber == command.PersonalNumber);

        Assert.That(person, Is.Not.Null);

        // Assert #3
        Assert.Multiple(() =>
        {
            Assert.That(person.FirstName, Is.EqualTo(command.FirstName.ToLower().Capitalize()));
            Assert.That(person.LastName, Is.EqualTo(command.LastName.ToLower().Capitalize()));
            Assert.That(person.PersonalNumber, Is.EqualTo(command.PersonalNumber));
            Assert.That(person.Gender, Is.EqualTo(command.Gender));
            Assert.That(person.CityCode, Is.EqualTo(command.CityCode));
            Assert.That(person.BirthDate, Is.EqualTo(command.BirthDate));
            Assert.That(person.PhoneNumbers, Has.Count.EqualTo(command.PhoneNumbers.Count));
            Assert.That(person.PhoneNumbers.Single().Number, Is.EqualTo(command.PhoneNumbers.Single().Number));
            Assert.That(person.PhoneNumbers.Single().Type, Is.EqualTo(command.PhoneNumbers.Single().Type));
            Assert.That(person.City.Code, Is.EqualTo(command.CityCode));
        });
    }
}