using AutoFixture;
using CivicHub.Application.Behaviours;
using CivicHub.Application.Common.Localization;
using CivicHub.Application.Common.Results;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;

namespace CivicHub.Application.UnitTests.Behaviours;

[TestFixture]
public class ValidationPipelineBehaviourTests
{
    private Mock<ILogger<ValidationPipelineBehaviour<TestCommand, Result>>> _loggerMock;
    private Mock<IValidator<TestCommand>> _validatorMock;
    private Mock<IValidationLocalizer> _validationLocalizerMock;
    private ValidationPipelineBehaviour<TestCommand, Result> _behaviour;
    private Fixture _fixture;
    private TestCommand _request;
    private CancellationToken _cancellationToken;

    [SetUp]
    public void SetUp()
    {
        _fixture = new Fixture();
        _loggerMock = new Mock<ILogger<ValidationPipelineBehaviour<TestCommand, Result>>>(MockBehavior.Loose);
        _validatorMock = new Mock<IValidator<TestCommand>>();
        _validationLocalizerMock = new Mock<IValidationLocalizer>();
        _behaviour =
            new ValidationPipelineBehaviour<TestCommand, Result>(_loggerMock.Object, _validationLocalizerMock.Object,
                _validatorMock.Object);
        _request = _fixture.Create<TestCommand>();
        _cancellationToken = CancellationToken.None;
        _validationLocalizerMock.Setup(x => x.Translate(It.IsAny<string>()))
            .Returns(string.Empty);
        _validationLocalizerMock.Setup(x => x.Translate(It.IsAny<string>(), It.IsAny<object>()))
            .Returns(string.Empty);
    }

    [Test]
    public async Task When_ValidatorIsNull_Then_ProceedsWithNextDelegate()
    {
        // Arrange
        var behaviour =
            new ValidationPipelineBehaviour<TestCommand, Result>(_loggerMock.Object, _validationLocalizerMock.Object);
        var expectedResponse = _fixture.Create<Result>();

        // Act
        var result = await behaviour.Handle(_request, () => Task.FromResult(expectedResponse), _cancellationToken);

        // Assert
        Assert.That(result, Is.EqualTo(expectedResponse));
    }

    [Test]
    public async Task When_ValidationPasses_Then_ProceedsWithNextDelegate()
    {
        // Arrange
        var expectedResponse = _fixture.Create<Result>();

        _validatorMock.Setup(v => v.ValidateAsync(_request, _cancellationToken))
            .ReturnsAsync(new ValidationResult());

        // Act
        var result = await _behaviour.Handle(_request, () => Task.FromResult(expectedResponse), _cancellationToken);

        // Assert
        Assert.That(result, Is.EqualTo(expectedResponse));
    }

    [Test]
    public async Task When_ValidationFails_Then_ReturnsErrors()
    {
        // Arrange
        var validationFailures = _fixture.CreateMany<ValidationFailure>(3).ToList();

        _validatorMock.Setup(v => v.ValidateAsync(_request, _cancellationToken))
            .ReturnsAsync(new ValidationResult(validationFailures));

        // Act
        var result = await _behaviour.Handle(
            _request,
            It.IsAny<RequestHandlerDelegate<Result>>(),
            _cancellationToken);

        // Assert
        var errors = result.Errors;
        Assert.Multiple(() =>
        {
            Assert.That(errors, Has.Count.EqualTo(validationFailures.Count));
            Assert.That(errors.All(e => e.Type == ErrorType.Validation), Is.True);
        });
    }

    [Test]
    public async Task When_ValidationFailsWithMultipleErrorsForSameProperty_Then_ReturnsAllErrors()
    {
        // Arrange
        var validationFailures = _fixture.CreateMany<ValidationFailure>(3).ToList();

        _validatorMock.Setup(v => v.ValidateAsync(_request, _cancellationToken))
            .ReturnsAsync(new ValidationResult(validationFailures));

        // Act
        var result = await _behaviour.Handle(
            _request,
            It.IsAny<RequestHandlerDelegate<Result>>(),
            _cancellationToken);

        // Assert
        Assert.That(result.Errors, Has.Count.EqualTo(validationFailures.Count));
    }

    [Test]
    public async Task When_MethodIsExecuted_Then_LogsHappen()
    {
        // Arrange
        var expectedResponse = _fixture.Create<Result>();

        _validatorMock.Setup(v => v.ValidateAsync(_request, _cancellationToken))
            .ReturnsAsync(new ValidationResult());

        // Act
        await _behaviour.Handle(_request, () => Task.FromResult(expectedResponse), _cancellationToken);

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

    public record TestCommand(string SomeProperty) : IRequest<Result>;
}