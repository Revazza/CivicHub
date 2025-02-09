using AutoFixture;
using CivicHub.Application.Features.PersonConnections.Commands.AddConnection;
using FluentValidation.TestHelper;
using NUnit.Framework;

namespace CivicHub.Application.UnitTests.Features.PersonConnections.Commands.AddConnection;

[TestFixture]
public class AddConnectionCommandValidatorTests
{
    private Fixture _fixture;
    private AddConnectionCommandValidator _validator;

    [SetUp]
    public void SetUp()
    {
        _fixture = new Fixture();
        _validator = new AddConnectionCommandValidator();
    }
    
    [Test]
    public void When_ValidCommandIsGiven_Then_ShouldNotHaveValidationErrors()
    {
        var command = _fixture.Build<AddConnectionCommand>()
            .With(x => x.PersonId, 1)
            .With(x => x.OtherPersonId, 2)
            .With(x => x.ConnectionType, "SomeConnectionType")
            .Create();
        
        var result = _validator.TestValidate(command);
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Test]
    public void When_PersonIdIsZeroOrNegative_Then_ShouldHaveValidationError()
    {
        var command = _fixture.Build<AddConnectionCommand>()
            .With(x => x.PersonId, 0)
            .With(x => x.OtherPersonId, 0)
            .Create();
        
        var result = _validator.TestValidate(command);
        
        result.ShouldHaveValidationErrorFor(c => c);
    }

    [TestCase(0)]
    [TestCase(-1)]
    public void When_OtherPersonIdIsZeroOrNegative_Then_ShouldHaveValidationError(int otherPersonId)
    {
        var command = _fixture.Build<AddConnectionCommand>()
            .With(x => x.PersonId, 1)
            .With(x => x.OtherPersonId, otherPersonId)
            .Create();
        
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(c => c.OtherPersonId);
    }
    
    [TestCase(0)]
    [TestCase(-1)]
    public void When_PersonIdIsZeroOrNegative_Then_ShouldHaveValidationError(int otherPersonId)
    {
        var command = _fixture.Build<AddConnectionCommand>()
            .With(x => x.PersonId, otherPersonId)
            .With(x => x.OtherPersonId, 1)
            .Create();
        
        var result = _validator.TestValidate(command);
        
        result.ShouldHaveValidationErrorFor(c => c.PersonId);
    }
}