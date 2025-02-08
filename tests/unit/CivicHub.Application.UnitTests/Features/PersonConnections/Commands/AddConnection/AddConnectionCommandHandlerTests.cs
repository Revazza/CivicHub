using AutoFixture;
using CivicHub.Application.Features.PersonConnections.Commands.AddConnection;
using CivicHub.Application.Repositories;
using CivicHub.Domain.Persons.Entities.PersonConnections;
using CivicHub.Domain.Persons.Entities.PersonConnections.Exceptions;
using CivicHub.Domain.Persons.Exceptions;
using Moq;
using NUnit.Framework;

namespace CivicHub.Application.UnitTests.Features.PersonConnections.Commands.AddConnection;

[TestFixture]
public class AddConnectionCommandHandlerTests
{
    private Fixture _fixture;
    private Mock<IUnitOfWork> _unitOfWorkMock;
    private Mock<IPersonRepository> _personRepositoryMock;
    private Mock<IPersonConnectionRepository> _personConnectionRepositoryMock;
    private AddConnectionCommandHandler _handler;

    [SetUp]
    public void SetUp()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _personRepositoryMock = new Mock<IPersonRepository>();
        _personConnectionRepositoryMock = new Mock<IPersonConnectionRepository>();
        _handler = new AddConnectionCommandHandler(_unitOfWorkMock.Object);

        _fixture = new Fixture();
        _unitOfWorkMock.Setup(u => u.PersonRepository).Returns(_personRepositoryMock.Object);
        _unitOfWorkMock.Setup(u => u.PersonConnectionRepository).Returns(_personConnectionRepositoryMock.Object);
    }

    [Test]
    public void When_PersonsDoNotExist_Then_ThrowsPersonDoesntExistException()
    {
        // Arrange
        var command = _fixture.Create<AddConnectionCommand>();
        _personRepositoryMock.Setup(pr =>
                pr.DoBothPersonsExistAsync(command.PersonId, command.OtherPersonId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        // Act & Assert
        Assert.ThrowsAsync<PersonDoesntExistException>(async () =>
            await _handler.Handle(command, CancellationToken.None));
    }

    [Test]
    public void When_ConnectionAlreadyExists_Then_ThrowsPersonConnectionExistsException()
    {
        // Arrange
        var command = _fixture.Create<AddConnectionCommand>();
        _personRepositoryMock.Setup(pr =>
                pr.DoBothPersonsExistAsync(command.PersonId, command.OtherPersonId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
        _personConnectionRepositoryMock.Setup(pcr =>
                pcr.DoesConnectionExistsAsync(command.PersonId, command.OtherPersonId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act & Assert
        Assert.ThrowsAsync<PersonConnectionExistsException>(async () =>
            await _handler.Handle(command, CancellationToken.None));
    }

    [Test]
    public async Task When_ValidCommandIsGiven_Then_AddsConnectionAndReturnsSuccess()
    {
        // Arrange
        var command = _fixture.Create<AddConnectionCommand>();
        _personRepositoryMock.Setup(pr =>
                pr.DoBothPersonsExistAsync(command.PersonId, command.OtherPersonId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
        _personConnectionRepositoryMock.Setup(pcr =>
                pcr.DoesConnectionExistsAsync(command.PersonId, command.OtherPersonId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.That(result.IsSuccess, Is.True);
        _personConnectionRepositoryMock.Verify(pcr =>
            pcr.InsertAsync(It.IsAny<PersonConnection>(), It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}