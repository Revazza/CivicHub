using AutoFixture;
using CivicHub.Application.Features.Persons.Commands.DeletePerson;
using CivicHub.Application.Repositories;
using CivicHub.Domain.Persons;
using CivicHub.Domain.Persons.Exceptions;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;

namespace CivicHub.Application.UnitTests.Features.Persons.Commands.DeletePerson;

[TestFixture]
public class DeletePersonCommandHandlerTests
{
    private Fixture _fixture;
    private Mock<IUnitOfWork> _unitOfWorkMock;
    private Mock<IPersonRepository> _personRepositoryMock;
    private Mock<IPersonConnectionRepository> _personConnectionRepositoryMock;
    private Mock<ILogger<DeletePersonCommandHandler>> _loggerMock;
    private DeletePersonCommandHandler _handler;

    [SetUp]
    public void SetUp()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _personRepositoryMock = new Mock<IPersonRepository>();
        _personConnectionRepositoryMock = new Mock<IPersonConnectionRepository>();
        _loggerMock = new Mock<ILogger<DeletePersonCommandHandler>>();
        _handler = new DeletePersonCommandHandler(_unitOfWorkMock.Object, _loggerMock.Object);

        _fixture = new Fixture();
        _fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        _unitOfWorkMock.Setup(u => u.PersonRepository).Returns(_personRepositoryMock.Object);
        _unitOfWorkMock.Setup(u => u.PersonConnectionRepository).Returns(_personConnectionRepositoryMock.Object);
    }

    [Test]
    public void When_PersonDoesNotExist_Then_ThrowsPersonDoesntExistException()
    {
        // Arrange
        var command = _fixture.Create<DeletePersonCommand>();
        _personRepositoryMock
            .Setup(pr => pr.GetByPersonalNumberAsync(command.PersonalNumber, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Person)null);

        // Act & Assert
        Assert.ThrowsAsync<PersonDoesntExistException>(async () =>
            await _handler.Handle(command, CancellationToken.None));
    }

    [Test]
    public async Task When_PersonExists_Then_DeletesPersonAndCommitsTransaction()
    {
        // Arrange
        var command = _fixture.Create<DeletePersonCommand>();
        var person = _fixture.Create<Person>();

        _personRepositoryMock
            .Setup(pr => pr.GetByPersonalNumberAsync(command.PersonalNumber, It.IsAny<CancellationToken>()))
            .ReturnsAsync(person);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.That(result.IsSuccess, Is.True);
        _unitOfWorkMock.Verify(u => u.BeginTransactionAsync(It.IsAny<CancellationToken>()), Times.Once);
        _personConnectionRepositoryMock.Verify(pc => pc.ExecuteDeleteAsync(person.Id, It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Exactly(2));
        _personRepositoryMock.Verify(pr => pr.Delete(person), Times.Once);
        _unitOfWorkMock.Verify(u => u.CommitTransactionAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test]
    public void When_DeletionFails_Then_RollsBackTransaction()
    {
        // Arrange
        var command = _fixture.Create<DeletePersonCommand>();
        var person = _fixture.Create<Person>();

        _personRepositoryMock
            .Setup(pr => pr.GetByPersonalNumberAsync(command.PersonalNumber, It.IsAny<CancellationToken>()))
            .ReturnsAsync(person);

        _unitOfWorkMock
            .Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Database error"));

        // Act & Assert
        Assert.ThrowsAsync<Exception>(async () =>
            await _handler.Handle(command, CancellationToken.None));

        _unitOfWorkMock.Verify(u => u.RollbackTransactionAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}
