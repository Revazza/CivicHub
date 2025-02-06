using AutoFixture;
using CivicHub.Application.Features.Persons.Commands.AddPerson;
using CivicHub.Application.Features.Persons.Commands.Common.Dtos;
using CivicHub.Domain.Persons;
using CivicHub.Domain.Persons.ValueObjects.PhoneNumbers.Enums;
using FluentValidation.Results;
using NUnit.Framework;

namespace CivicHub.Application.UnitTests.Features.Persons.Commands.AddPerson;

[TestFixture]
public class AddPersonCommandValidatorTests
{
    private AddPersonCommandValidator _validator;
    private Fixture _fixture;

    [SetUp]
    public void SetUp()
    {
        _fixture = new Fixture();
        _validator = new AddPersonCommandValidator();
    }

    [TestCase("")]
    [TestCase(null)]
    [TestCase("123")]
    [TestCase("s")]
    [TestCase("ქ")]
    [TestCase("wrong input")]
    [TestCase("english5")]
    [TestCase("@$%#")]
    [TestCase("ქართულიდაenglish")]
    [TestCase("ქართული1")]
    [TestCase("სა@")]
    public void When_FirstNameIsInvalid_Then_ValidationFails(string invalidFirstName)
    {
        // Arrange
        var command = _fixture.Build<AddPersonCommand>()
            .With(c => c.FirstName, invalidFirstName)
            .Create();

        // Act
        var result = _validator.Validate(command);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.IsValid, Is.False);
            var errorMessage = FindErrorMessage(nameof(Person.FirstName), result);
            Assert.That(errorMessage, Is.Not.Null);
        });
    }

    [TestCase("")]
    [TestCase(null)]
    [TestCase("123")]
    [TestCase("s")]
    [TestCase("ქ")]
    [TestCase("wrong input")]
    [TestCase("english5")]
    [TestCase("@$%#")]
    [TestCase("ქართულიდაenglish")]
    [TestCase("ქართული1")]
    [TestCase("სა@")]
    public void When_LastNameIsInvalid_Then_ValidationFails(string lastName)
    {
        // Arrange
        var command = _fixture.Build<AddPersonCommand>()
            .With(c => c.LastName, lastName)
            .Create();

        // Act
        var result = _validator.Validate(command);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.IsValid, Is.False);
            var errorMessage = FindErrorMessage(nameof(Person.LastName), result);
            Assert.That(errorMessage, Is.Not.Null);
        });
    }

    [TestCase("")]
    [TestCase(null)]
    [TestCase("123")]
    [TestCase("s")]
    [TestCase("ქ")]
    [TestCase("wrong input")]
    [TestCase("english5")]
    [TestCase("@$%#")]
    [TestCase("ქართულიდაenglish")]
    [TestCase("ქართული1")]
    [TestCase("სა@")]
    [TestCase("-2324")]
    public void When_PersonalNumberIsInvalid_Then_ValidationFails(string invalidPersonalNumber)
    {
        // Arrange
        var command = _fixture.Build<AddPersonCommand>()
            .With(c => c.PersonalNumber, invalidPersonalNumber)
            .Create();

        // Act
        var result = _validator.Validate(command);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.IsValid, Is.False);
            var errorMessage = FindErrorMessage(nameof(Person.PersonalNumber), result);
            Assert.That(errorMessage, Is.Not.Null);
        });
    }

    [TestCase(2050, 01, 01)] // Future date
    [TestCase(2020, 01, 01)] // Not adult
    [TestCase(1121, 08, 12)] // Magic age
    public void When_AgeIsInvalid_Then_ValidationFails(int years, int month, int day)
    {
        // Arrange
        var command = _fixture.Build<AddPersonCommand>()
            .With(c => c.BirthDate, new DateTime(years, month, day))
            .Create();

        // Act
        var result = _validator.Validate(command);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.IsValid, Is.False);
            var errorMessage = FindErrorMessage(nameof(Person.BirthDate), result);
            Assert.That(errorMessage, Is.Not.Null);
        });
    }

    [TestCase("")]
    [TestCase(null)]
    [TestCase("o k")]
    [TestCase("o   ")]
    [TestCase(" o")]
    [TestCase(" o ")]
    [TestCase("o ")]
    [TestCase("123")]
    [TestCase("ქა")]
    public void When_CityCodeIsInvalid_Then_ValidationFails(string invalidCityCode)
    {
        // Arrange
        var command = _fixture.Build<AddPersonCommand>()
            .With(c => c.CityCode, invalidCityCode)
            .Create();

        // Act
        var result = _validator.Validate(command);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.IsValid, Is.False);
            var errorMessage = FindErrorMessage(nameof(Person.CityCode), result);
            Assert.That(errorMessage, Is.Not.Null);
        });
    }

    [TestCase("")]
    [TestCase("  ")]
    [TestCase("a")]
    [TestCase("1234567")]
    public void When_PhoneNumbersCountryIsInvalid_Then_ValidationFails(string countryCode)
    {
        // Arrange
        var command = _fixture.Build<AddPersonCommand>()
            .With(c => c.PhoneNumbers, [new PhoneNumberDto(countryCode, "568", "117764", PhoneType.Home)])
            .Create();

        // Act
        var result = _validator.Validate(command);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.IsValid, Is.False);
            var errorMessage = FindErrorMessage(nameof(PhoneNumberDto.CountryCode), result);
            Assert.That(errorMessage, Is.Not.Null);
        });
    }

    [TestCase("")]
    [TestCase("  ")]
    [TestCase("a")]
    [TestCase("1234567")]
    public void When_PhoneNumbersAreaCodeIsInvalid_Then_ValidationFails(string areaCode)
    {
        // Arrange
        var command = _fixture.Build<AddPersonCommand>()
            .With(c => c.PhoneNumbers, [new PhoneNumberDto("995", areaCode, "117764", PhoneType.Home)])
            .Create();

        // Act
        var result = _validator.Validate(command);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.IsValid, Is.False);
            var errorMessage = FindErrorMessage(nameof(PhoneNumberDto.AreaCode), result);
            Assert.That(errorMessage, Is.Not.Null);
        });
    }

    [TestCase("")]
    [TestCase("  ")]
    [TestCase("a")]
    [TestCase("123")]
    [TestCase("123456789876554")]
    public void When_PhoneNumbersNumberIsInvalid_Then_ValidationFails(string number)
    {
        // Arrange
        var command = _fixture.Build<AddPersonCommand>()
            .With(c => c.PhoneNumbers, [new PhoneNumberDto("995", "568", number, PhoneType.Home)])
            .Create();

        // Act
        var result = _validator.Validate(command);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.IsValid, Is.False);
            var errorMessage = FindErrorMessage(nameof(PhoneNumberDto.Number), result);
            Assert.That(errorMessage, Is.Not.Null);
        });
    }

    [Test]
    public void When_ValidCommand_Then_ValidationSucceeds()
    {
        // Arrange
        var command = _fixture.Build<AddPersonCommand>()
            .With(c => c.FirstName, "John")
            .With(c => c.LastName, "Doe")
            .With(c => c.PersonalNumber, "11111111111")
            .With(c => c.BirthDate, new DateTime(2000, 01, 01))
            .With(c => c.CityCode, "TB")
            .With(c => c.PhoneNumbers, [new PhoneNumberDto("995", "568", "117764", PhoneType.Home)])
            .Create();

        // Act
        var result = _validator.Validate(command);

        // Assert
        Assert.That(result.IsValid, Is.True);
    }

    private static string FindErrorMessage(string propertyName, ValidationResult validationResult)
        => validationResult
            .Errors
            .FirstOrDefault(validationFailure =>
                validationFailure.PropertyName.Contains(propertyName))?.ErrorMessage;
}