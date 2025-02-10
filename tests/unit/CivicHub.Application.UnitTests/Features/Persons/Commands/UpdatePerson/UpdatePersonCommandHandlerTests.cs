using AutoFixture;
using AutoFixture.Dsl;
using CivicHub.Application.Common.Extensions;
using CivicHub.Application.Features.Persons.Commands.Common.Dtos;
using CivicHub.Application.Features.Persons.Commands.UpdatePerson;
using CivicHub.Application.Repositories;
using CivicHub.Domain.Cities.Exceptions;
using CivicHub.Domain.Persons;
using CivicHub.Domain.Persons.Enums;
using CivicHub.Domain.Persons.Exceptions;
using CivicHub.Domain.Persons.ValueObjects.PhoneNumbers;
using CivicHub.Domain.Persons.ValueObjects.PhoneNumbers.Enums;
using CivicHub.Domain.Persons.ValueObjects.PhoneNumbers.Exceptions;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;

namespace CivicHub.Application.UnitTests.Features.Persons.Commands.UpdatePerson;

[TestFixture]
public class UpdatePersonCommandHandlerTests
{
    private const string PersonalNumber = "PersonalNumber";
    private const string CityCode = "CityCode";
    private Mock<IUnitOfWork> _unitOfWorkMock;
    private Mock<IPersonRepository> _personRepositoryMock;
    private Mock<ICityRepository> _cityRepositoryMock;
    private Mock<ILogger<UpdatePersonCommandHandler>> _loggerMock;
    private Fixture _fixture;
    private UpdatePersonCommandHandler _handler;
    private IPostprocessComposer<UpdatePersonCommand> _commandComposer;
    private IPostprocessComposer<Person> _personComposer;


    [SetUp]
    public void SetUp()
    {
        _fixture = new Fixture();
        _personRepositoryMock = new Mock<IPersonRepository>();
        _cityRepositoryMock = new Mock<ICityRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _loggerMock = new Mock<ILogger<UpdatePersonCommandHandler>>();
        _handler = new UpdatePersonCommandHandler(_unitOfWorkMock.Object, _loggerMock.Object);

        _fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

        _commandComposer = _fixture
            .Build<UpdatePersonCommand>()
            .With(x => x.CityCode, CityCode)
            .With(x => x.PersonalNumber, PersonalNumber);

        _personComposer = _fixture
            .Build<Person>()
            .With(x => x.CityCode, CityCode);

        _unitOfWorkMock.Setup(u => u.PersonRepository).Returns(_personRepositoryMock.Object);
        _unitOfWorkMock.Setup(u => u.CityRepository).Returns(_cityRepositoryMock.Object);

        var tempPerson = _personComposer.Create();
        _personRepositoryMock
            .Setup(pr => pr.GetForUpdateAsync(tempPerson.PersonalNumber, It.IsAny<CancellationToken>()))
            .ReturnsAsync(tempPerson);

        _cityRepositoryMock.Setup(cr =>
                cr.DoesExistAsync(_commandComposer.Create().CityCode, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
    }

    [Order(0)]
    [Test]
    public async Task When_ValidUpdateCommand_Then_PersonIsUpdatedSuccessfully()
    {
        // Arrange
        var command = _commandComposer
            .With(c => c.Gender, Gender.Male)
            .Create();

        var person = _personComposer
            .With(c => c.Gender, Gender.Female)
            .Create();

        _personRepositoryMock.Setup(pr => pr.GetForUpdateAsync(command.PersonalNumber, It.IsAny<CancellationToken>()))
            .ReturnsAsync(person);

        _cityRepositoryMock.Setup(cr => cr.DoesExistAsync(command.CityCode, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        _unitOfWorkMock.Verify(pr => pr.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);

        Assert.Multiple(() =>
        {
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(person.FirstName, Is.EqualTo(command.FirstName.ToLower().Capitalize()));
            Assert.That(person.LastName, Is.EqualTo(command.LastName.ToLower().Capitalize()));
            Assert.That(person.CityCode, Is.EqualTo(command.CityCode));
            Assert.That(person.Gender, Is.EqualTo(command.Gender));
            Assert.That(person.BirthDate, Is.EqualTo(command.BirthDate));
        });
    }

    [Test]
    public void When_PersonDoesNotExist_Then_ThrowsPersonDoesntExistException()
    {
        // Arrange
        var command = _commandComposer.Create();

        _personRepositoryMock.Setup(pr => pr.GetForUpdateAsync(command.PersonalNumber, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Person)null);

        // Act & Assert
        Assert.ThrowsAsync<PersonDoesntExistException>(
            async () => await _handler.Handle(command, CancellationToken.None));
    }

    [Test]
    public void When_CityCodeDoesNotExist_Then_ThrowsCityDoesntExistException()
    {
        // Arrange
        var command = _commandComposer
            .Create();

        var person = _personComposer
            .With(x => x.CityCode,string.Empty)
            .Create();

        _personRepositoryMock.Setup(pr => pr.GetForUpdateAsync(command.PersonalNumber, It.IsAny<CancellationToken>()))
            .ReturnsAsync(person);

        _cityRepositoryMock.Setup(cr => cr.DoesExistAsync(command.CityCode, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        // Act & Assert
        Assert.ThrowsAsync<CityDoesntExistException>(
            async () => await _handler.Handle(command, CancellationToken.None));
    }

    [TestCase(null)]
    [TestCase("")]
    public async Task When_CityCodeIsNullOrEmpty_Then_NoUpdateOccurs(string newCityCode)
    {
        // Arrange
        var command = _commandComposer
            .With(c => c.CityCode, newCityCode)
            .Create();

        var person = _personComposer
            .Create();

        _personRepositoryMock.Setup(pr => pr.GetForUpdateAsync(command.PersonalNumber, It.IsAny<CancellationToken>()))
            .ReturnsAsync(person);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.That(person.CityCode, Is.Not.EqualTo(newCityCode));
    }

    [TestCase(null)]
    [TestCase("")]
    public async Task When_UpdateFirstNameIsNullOrEmpty_Then_NoUpdateOccurs(string newFirstName)
    {
        // Arrange
        var command = _commandComposer
            .With(c => c.FirstName, newFirstName)
            .Create();

        var person = _personComposer
            .Create();

        _personRepositoryMock.Setup(pr => pr.GetForUpdateAsync(command.PersonalNumber, It.IsAny<CancellationToken>()))
            .ReturnsAsync(person);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.That(person.FirstName, Is.Not.EqualTo(newFirstName));
    }

    [TestCase(null)]
    [TestCase("")]
    public async Task When_UpdateLastNameIsNullOrEmpty_Then_NoUpdateOccurs(string newLastName)
    {
        // Arrange
        var command = _commandComposer
            .With(c => c.FirstName, newLastName)
            .Create();

        var person = _personComposer
            .Create();

        _personRepositoryMock.Setup(pr => pr.GetForUpdateAsync(command.PersonalNumber, It.IsAny<CancellationToken>()))
            .ReturnsAsync(person);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.That(person.FirstName, Is.Not.EqualTo(newLastName));
    }

    [Test]
    public async Task When_GenderIsNotSpecified_Then_NoUpdateOccurs()
    {
        // Arrange
        var command = _commandComposer
            .With(c => c.Gender, Gender.NotSpecified)
            .Create();

        var person = _personComposer
            .With(p => p.Gender, Gender.Male)
            .Create();

        _personRepositoryMock.Setup(pr => pr.GetForUpdateAsync(command.PersonalNumber, It.IsAny<CancellationToken>()))
            .ReturnsAsync(person);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.That(person.Gender, Is.EqualTo(Gender.Male));
    }
}