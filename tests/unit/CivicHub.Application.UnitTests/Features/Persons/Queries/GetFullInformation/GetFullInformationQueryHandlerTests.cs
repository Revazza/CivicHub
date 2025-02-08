using AutoFixture;
using CivicHub.Application.Features.Persons.Queries.GetFullInformation;
using CivicHub.Application.Repositories;
using Moq;
using NUnit.Framework;

namespace CivicHub.Application.UnitTests.Features.Persons.Queries.GetFullInformation;

[TestFixture]
public class GetFullInformationQueryHandlerTests
{
    private Fixture _fixture;
    private Mock<IUnitOfWork> _unitOfWorkMock;
    private Mock<IPersonRepository> _personRepositoryMock;
    private GetFullInformationQueryHandler _handler;

    [SetUp]
    public void SetUp()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _personRepositoryMock = new Mock<IPersonRepository>();
        _handler = new GetFullInformationQueryHandler(_unitOfWorkMock.Object);

        _fixture = new Fixture();
        _unitOfWorkMock.Setup(u => u.PersonRepository).Returns(_personRepositoryMock.Object);
    }

    [Test]
    public async Task When_PersonDoesNotExist_Then_ReturnsNull()
    {
        // Arrange
        var query = _fixture.Create<GetFullInformationQuery>();

        _personRepositoryMock.Setup(x => x.GetPersonFullInformationAsync(query.PersonId, CancellationToken.None))
            .ReturnsAsync((GetFullInformationResponse)null);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.That(result.Value, Is.Null);
    }

    [Test]
    public async Task When_PersonExists_Then_ReturnsFullInformationResponse()
    {
        // Arrange
        var query = _fixture.Create<GetFullInformationQuery>();

        var info = _fixture.Create<GetFullInformationResponse>();

        _personRepositoryMock.Setup(x => x.GetPersonFullInformationAsync(query.PersonId, CancellationToken.None))
            .ReturnsAsync(info);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.That(result.Value, Is.EqualTo(info));
    }
}