using AutoFixture;
using AutoFixture.Dsl;
using CivicHub.Application.Features.Persons.Commands.AddPerson;
using CivicHub.Application.Features.Persons.Commands.Common.Dtos;
using CivicHub.Domain.Persons.ValueObjects.PhoneNumbers.Enums;
using FluentValidation.TestHelper;
using NUnit.Framework;

namespace CivicHub.Application.UnitTests.Features.Persons.Commands.AddPerson;

[TestFixture]
public class AddPersonCommandValidatorTests
{
    private AddPersonCommandValidator _validator;
    private Fixture _fixture;
    private IPostprocessComposer<AddPersonCommand> _validCommandComposer;

    [SetUp]
    public void SetUp()
    {
        _fixture = new Fixture();
        _validator = new AddPersonCommandValidator();

        // Update composer only in the test methods
        _validCommandComposer = _fixture.Build<AddPersonCommand>()
            .With(c => c.FirstName, "John")
            .With(c => c.LastName, "Doe")
            .With(c => c.PersonalNumber, "11111111111")
            .With(c => c.BirthDate, new DateTime(2000, 01, 01))
            .With(c => c.CityCode, "TB")
            .With(c => c.PhoneNumbers, [new PhoneNumberDto("995", "568", "117764", PhoneType.Home)]);
    }

    [Order(0)]
    [Test]
    public void When_ValidCommand_Then_ValidationSucceeds()
    {
        // Arrange
        var command = _validCommandComposer.Create();

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        Assert.That(result.IsValid, Is.True);
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
        var command = _validCommandComposer
            .With(c => c.FirstName, invalidFirstName)
            .Create();

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.FirstName);
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
        var command = _validCommandComposer
            .With(c => c.LastName, lastName)
            .Create();

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.LastName);
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
        var command = _validCommandComposer
            .With(c => c.PersonalNumber, invalidPersonalNumber)
            .Create();

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.PersonalNumber);
    }

    [TestCase(2050, 01, 01)] // Future date
    [TestCase(2020, 01, 01)] // Not adult
    [TestCase(1121, 08, 12)] // Magic age
    public void When_AgeIsInvalid_Then_ValidationFails(int years, int month, int day)
    {
        // Arrange
        var command = _validCommandComposer
            .With(c => c.BirthDate, new DateTime(years, month, day))
            .Create();

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.BirthDate);
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
        var command = _validCommandComposer
            .With(c => c.CityCode, invalidCityCode)
            .Create();

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.CityCode);
    }

    [TestCase("")]
    [TestCase("  ")]
    [TestCase("a")]
    [TestCase("1234567")]
    public void When_PhoneNumbersCountryIsInvalid_Then_ValidationFails(string countryCode)
    {
        // Arrange
        var command = _validCommandComposer
            .With(c => c.PhoneNumbers, [new PhoneNumberDto(countryCode, "568", "117764", PhoneType.Home)])
            .Create();

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor($"PhoneNumbers[0].{nameof(PhoneNumberDto.CountryCode)}");
    }

    [TestCase("")]
    [TestCase("  ")]
    [TestCase("a")]
    [TestCase("1234567")]
    public void When_PhoneNumbersAreaCodeIsInvalid_Then_ValidationFails(string areaCode)
    {
        // Arrange
        var command = _validCommandComposer
            .With(c => c.PhoneNumbers, [new PhoneNumberDto("995", areaCode, "117764", PhoneType.Home)])
            .Create();

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor($"PhoneNumbers[0].{nameof(PhoneNumberDto.AreaCode)}");
    }

    [TestCase("")]
    [TestCase(null)]
    [TestCase("  ")]
    [TestCase("a")]
    [TestCase("123a")]
    [TestCase("123456789876554")]
    public void When_PhoneNumbersNumberIsInvalid_Then_ValidationFails(string number)
    {
        // Arrange
        var command = _validCommandComposer
            .With(c => c.PhoneNumbers, [new PhoneNumberDto("995", "568", number, PhoneType.Home)])
            .Create();

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor($"PhoneNumbers[0].{nameof(PhoneNumberDto.Number)}");
    }

    [Test]
    public void When_PhoneNumbersAreNull_Then_ValidationFails()
    {
        // Arrange
        var command = _validCommandComposer
            .With(c => c.PhoneNumbers, (List<PhoneNumberDto>)null)
            .Create();

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.PhoneNumbers);
    }

    [Test]
    public void When_PhoneNumbersAreEmpty_Then_ValidationFails()
    {
        // Arrange
        var command = _validCommandComposer
            .With(c => c.PhoneNumbers, (List<PhoneNumberDto>)null)
            .Create();

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.PhoneNumbers);
    }
}