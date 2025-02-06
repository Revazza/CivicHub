using AutoFixture;
using CivicHub.Application.Features.Persons.Commands.AddPerson;
using CivicHub.Application.Repositories;
using CivicHub.Domain.Cities.Exceptions;
using CivicHub.Domain.Persons;
using CivicHub.Domain.Persons.Exceptions;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;

namespace CivicHub.Application.UnitTests.Features.Persons.Commands.AddPerson;

[TestFixture]
public class AddPersonCommandHandlerTests
{
    private Mock<IUnitOfWork> _unitOfWorkMock;
    private Mock<IPersonRepository> _personRepositoryMock;
    private Mock<ICityRepository> _cityRepositoryMock;
    private Mock<ILogger<AddPersonCommandHandler>> _loggerMock;
    private Fixture _fixture;
    private AddPersonCommandHandler _handler;


    [SetUp]
    public void SetUp()
    {
        _fixture = new Fixture();
        _personRepositoryMock = new Mock<IPersonRepository>();
        _cityRepositoryMock = new Mock<ICityRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _loggerMock = new Mock<ILogger<AddPersonCommandHandler>>();
        _handler = new AddPersonCommandHandler(_unitOfWorkMock.Object, _loggerMock.Object);

        _unitOfWorkMock.Setup(u => u.PersonRepository).Returns(_personRepositoryMock.Object);
        _unitOfWorkMock.Setup(u => u.CityRepository).Returns(_cityRepositoryMock.Object);
    }

    [Test]
    public void When_PersonalNumberAlreadyExists_Then_ThrowsPersonAlreadyExistsException()
    {
        // Arrange
        var command = _fixture.Build<AddPersonCommand>()
            .With(c => c.PersonalNumber, "12345")
            .Create();

        _personRepositoryMock.Setup(pr => pr.DoesExistAsync(command.PersonalNumber, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act & Assert
        Assert.ThrowsAsync<PersonAlreadyExistsException>(
            async () => await _handler.Handle(command, CancellationToken.None));
    }

    [Test]
    public void When_CityDoesNotExist_Then_ThrowsCityDoesntExistException()
    {
        // Arrange
        var command = _fixture.Build<AddPersonCommand>()
            .With(c => c.CityCode, "NonExistentCity")
            .Create();

        _cityRepositoryMock.Setup(cr => cr.DoesExistAsync(command.CityCode, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        // Act & Assert
        Assert.ThrowsAsync<CityDoesntExistException>(
            async () => await _handler.Handle(command, CancellationToken.None));
    }

    [Test]
    public async Task When_ValidCommand_Then_PersonIsAddedSuccessfully()
    {
        // Arrange
        var command = _fixture.Create<AddPersonCommand>();

        _personRepositoryMock.Setup(pr => pr.DoesExistAsync(command.PersonalNumber, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        _cityRepositoryMock.Setup(cr => cr.DoesExistAsync(command.CityCode, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.That(result.IsSuccess, Is.True);
        _personRepositoryMock.Verify(p => p.InsertAsync(It.IsAny<Person>(), It.IsAny<CancellationToken>()));
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test]
    public async Task When_AddPersonCommandIsHandled_Then_LogsCorrectInformation()
    {
        // Arrange
        var command = _fixture.Create<AddPersonCommand>();

        _personRepositoryMock.Setup(pr => pr.DoesExistAsync(command.PersonalNumber, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        _cityRepositoryMock.Setup(cr => cr.DoesExistAsync(command.CityCode, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _loggerMock.Verify(logger =>
                logger.Log(
                    It.IsAny<LogLevel>(),
                    It.IsAny<EventId>(),
                    It.IsAny<It.IsAnyType>(),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>())
            , Times.Exactly(4));
    }
}