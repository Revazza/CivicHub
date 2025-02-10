using AutoFixture;
using AutoFixture.Dsl;
using CivicHub.Application.Common.Extensions;
using CivicHub.Application.Features.Persons.Commands.Common.Dtos;
using CivicHub.Application.Features.Persons.Commands.UpdatePerson;
using CivicHub.Application.IntegrationTestsCommon;
using CivicHub.Domain.Cities;
using CivicHub.Domain.Persons;
using CivicHub.Domain.Persons.Enums;
using CivicHub.Domain.Persons.ValueObjects.PhoneNumbers;
using CivicHub.Domain.Persons.ValueObjects.PhoneNumbers.Enums;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace CivicHub.Application.IntegrationTests.Features.Persons.Commands.UpdatePerson;

public class UpdatePersonCommandHandlerTests : IntegrationTestBase
{
    private IPostprocessComposer<UpdatePersonCommand> _validCommandComposer;
    private Person _existingPerson;

    [SetUp]
    public async Task SetUp()
    {
        var initialPhoneNumber = Fixture
            .Build<PhoneNumber>()
            .With(x => x.Number, "1111")
            .With(x => x.Type, PhoneType.Mobile)
            .Create();

        var initialCity = Fixture.Build<City>()
            .With(x => x.Code, "TB")
            .With(x => x.Name, "Tbilisi")
            .Create();

        _existingPerson = Fixture.Build<Person>()
            .With(x => x.FirstName, "GeOrGe")
            .With(x => x.LastName, "SmItH")
            .With(x => x.PersonalNumber, "12345678900")
            .With(x => x.City, initialCity)
            .With(x => x.Gender, Gender.Male)
            .With(x => x.BirthDate, DateTime.UtcNow.AddYears(-30))
            .With(x => x.PhoneNumbers, [initialPhoneNumber])
            .Create();

        var newCity = Fixture.Build<City>()
            .With(x => x.Code, "BT")
            .With(x => x.Name, "Batumi")
            .Create();

        var newPhoneNumber = Fixture
            .Build<PhoneNumberDto>()
            .With(x => x.Number, "5555")
            .With(x => x.Type, PhoneType.Home)
            .Create();

        _validCommandComposer = Fixture.Build<UpdatePersonCommand>()
            .With(x => x.PersonalNumber, _existingPerson.PersonalNumber)
            .With(x => x.FirstName, "JoHn")
            .With(x => x.LastName, "DoE")
            .With(x => x.CityCode, newCity.Code)
            .With(x => x.Gender, Gender.Female)
            .With(x => x.BirthDate, DateTime.UtcNow.AddYears(-25))
            .With(x => x.PhoneNumbers, [newPhoneNumber]);

        await Context.Cities.AddRangeAsync(initialCity, newCity);
        await Context.Persons.AddAsync(_existingPerson);
        await Context.SaveChangesAsync();
    }

    [Test]
    public async Task When_ValidCommand_Then_PersonIsUpdatedCorrectly()
    {
        // Arrange
        var command = _validCommandComposer.Create();

        // Act
        var result = await Sender.Send(command);

        // Assert
        Assert.That(result.IsSuccess, Is.True);

        var updatedPerson = await Context
            .Persons
            .Include(x => x.PhoneNumbers)
            .Include(x => x.City)
            .SingleOrDefaultAsync(p => p.PersonalNumber == command.PersonalNumber);

        // Assert #2
        Assert.That(updatedPerson, Is.Not.Null);

        // Assert #3
        var newPhoneNumber = command.PhoneNumbers.Single();
        var personPhoneNumber = updatedPerson.PhoneNumbers.Single(x => x.Type == newPhoneNumber.Type);
        Assert.Multiple(() =>
        {
            Assert.That(updatedPerson.FirstName, Is.EqualTo(command.FirstName.ToLower().Capitalize()));
            Assert.That(updatedPerson.LastName, Is.EqualTo(command.LastName.ToLower().Capitalize()));
            Assert.That(updatedPerson.PersonalNumber, Is.EqualTo(command.PersonalNumber));
            Assert.That(updatedPerson.Gender, Is.EqualTo(command.Gender));
            Assert.That(updatedPerson.CityCode, Is.EqualTo(command.CityCode));
            Assert.That(updatedPerson.BirthDate, Is.EqualTo(command.BirthDate));
            Assert.That(personPhoneNumber.Number, Is.EqualTo(newPhoneNumber.Number));
            Assert.That(personPhoneNumber.Type, Is.EqualTo(newPhoneNumber.Type));
            Assert.That(updatedPerson.City.Code, Is.EqualTo(command.CityCode));
        });
    }

    [Test]
    public async Task When_NoUpdateFieldsProvided_Then_PersonRemainsUnchanged()
    {
        // Arrange
        var command = new UpdatePersonCommand(PersonalNumber: _existingPerson.PersonalNumber);

        // Act
        var result = await Sender.Send(command);

        // Assert
        Assert.That(result.IsSuccess, Is.True);

        var unchangedPerson = await Context
            .Persons
            .Include(x => x.PhoneNumbers)
            .SingleAsync(p => p.PersonalNumber == command.PersonalNumber);

        Assert.Multiple(() =>
        {
            Assert.That(unchangedPerson.FirstName, Is.EqualTo(_existingPerson.FirstName));
            Assert.That(unchangedPerson.LastName, Is.EqualTo(_existingPerson.LastName));
            Assert.That(unchangedPerson.Gender, Is.EqualTo(_existingPerson.Gender));
            Assert.That(unchangedPerson.CityCode, Is.EqualTo(_existingPerson.CityCode));
            Assert.That(unchangedPerson.BirthDate, Is.EqualTo(_existingPerson.BirthDate));
            Assert.That(unchangedPerson.PhoneNumbers, Has.Count.EqualTo(_existingPerson.PhoneNumbers.Count));
        });
    }
}