using AutoFixture;
using AutoFixture.Dsl;
using CivicHub.Application.Features.Persons.Commands.Common.Dtos;
using CivicHub.Application.Features.Persons.Commands.UpdatePerson;
using CivicHub.Domain.Persons.ValueObjects.PhoneNumbers.Enums;
using FluentValidation.TestHelper;
using NUnit.Framework;

namespace CivicHub.Application.UnitTests.Features.Persons.Commands.UpdatePerson;

[TestFixture]
public class UpdatePersonCommandValidatorTests
{
    private UpdatePersonCommandValidator _validator;
    private Fixture _fixture;
    private IPostprocessComposer<UpdatePersonCommand> _validCommandComposer;

    [SetUp]
    public void SetUp()
    {
        _fixture = new Fixture();
        _validator = new UpdatePersonCommandValidator();

        // Update composer only in the test methods
        _validCommandComposer = _fixture.Build<UpdatePersonCommand>()
            .With(c => c.FirstName, "John")
            .With(c => c.LastName, "Doe")
            .With(c => c.PersonalNumber, "11111111111")
            .With(c => c.BirthDate, new DateTime(2000, 01, 01))
            .With(c => c.CityCode, "TB")
            .With(c => c.PhoneNumbers, [new PhoneNumberDto("117764", PhoneType.Home)]);
    }

    [Order(0)]
    [Test]
    public void When_ValidCommand_Then_ValidationSucceeds()
    {
        // Arrange
        var command = _validCommandComposer.Create();

        // Act
        var result = _validator.Validate(command);

        // Assert
        Assert.That(result.IsValid, Is.True);
    }

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
        result.ShouldHaveValidationErrorFor("BirthDate.Age");
    }

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
        result.ShouldHaveValidationErrorFor("CityCode.CityCode");
    }

    [TestCase("")]
    [TestCase(null)]
    [TestCase("  ")]
    [TestCase("a")]
    [TestCase("123a")]
    public void When_PhoneNumbersNumberIsInvalid_Then_ValidationFails(string number)
    {
        // Arrange
        var command = _validCommandComposer
            .With(c => c.PhoneNumbers, [new PhoneNumberDto(number, PhoneType.Home)])
            .Create();

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor($"PhoneNumbers[0].{nameof(PhoneNumberDto.Number)}");
    }

    [Test]
    public void When_PhoneNumbersAreNull_Then_ValidationSkips()
    {
        // Arrange
        var command = _validCommandComposer
            .With(c => c.PhoneNumbers, (List<PhoneNumberDto>)null)
            .Create();

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.PhoneNumbers);
    }
    
    [Test]
    public void When_PhoneNumbersAreEmpty_Then_ValidationSkips()
    {
        // Arrange
        var command = _validCommandComposer
            .With(c => c.PhoneNumbers, [])
            .Create();

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.PhoneNumbers);
    }
}