using AutoFixture;
using CivicHub.Application.Features.PersonConnections.Commands.RemoveConnection;
using CivicHub.Application.Repositories;
using CivicHub.Domain.Persons.Entities.PersonConnections;
using CivicHub.Domain.Persons.Entities.PersonConnections.Exceptions;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;

namespace CivicHub.Application.UnitTests.Features.PersonConnections.Commands.RemoveConnection;

[TestFixture]
public class RemoveConnectionCommandHandlerTests
{
    private Fixture _fixture;
    private Mock<IUnitOfWork> _unitOfWorkMock;
    private Mock<IPersonConnectionRepository> _personConnectionRepositoryMock;
    private Mock<ILogger<RemoveConnectionCommandHandler>> _loggerMock;
    private RemoveConnectionCommandHandler _handler;

    [SetUp]
    public void SetUp()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _personConnectionRepositoryMock = new Mock<IPersonConnectionRepository>();
        _loggerMock = new Mock<ILogger<RemoveConnectionCommandHandler>>();
        _handler = new RemoveConnectionCommandHandler(_unitOfWorkMock.Object, _loggerMock.Object);

        _fixture = new Fixture();
        
        _fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

        _unitOfWorkMock.Setup(u => u.PersonConnectionRepository).Returns(_personConnectionRepositoryMock.Object);
    }

    [Test]
    public void When_ConnectionDoesNotExist_Then_ThrowsPersonConnectionDoesntExistException()
    {
        // Arrange
        var command = _fixture.Create<RemoveConnectionCommand>();
        _personConnectionRepositoryMock.Setup(pr =>
                pr.GetByIdAsync(command.ConnectionId))
            .ReturnsAsync((PersonConnection)null);

        // Act & Assert
        Assert.ThrowsAsync<PersonConnectionDoesntExistException>(async () =>
            await _handler.Handle(command, CancellationToken.None));
    }

    [Test]
    public async Task When_ConnectionExists_Then_DeletesConnectionAndReturnsSuccess()
    {
        // Arrange
        var command = _fixture.Create<RemoveConnectionCommand>();
        var existingConnection = _fixture.Create<PersonConnection>();
        
        _personConnectionRepositoryMock.Setup(pr =>
                pr.GetByIdAsync(command.ConnectionId))
            .ReturnsAsync(existingConnection);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.That(result.IsSuccess, Is.True);
        _personConnectionRepositoryMock.Verify(pr => pr.Delete(existingConnection), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(CancellationToken.None), Times.Once);
    }
    
    [Test]
    public async Task When_ConnectionExists_Then_DeletesConnectionAndLog()
    {
        // Arrange
        var command = _fixture.Create<RemoveConnectionCommand>();
        var existingConnection = _fixture.Create<PersonConnection>();
        
        _personConnectionRepositoryMock.Setup(pr =>
                pr.GetByIdAsync(command.ConnectionId))
            .ReturnsAsync(existingConnection);

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
            , Times.AtLeast(1));
    }
}
