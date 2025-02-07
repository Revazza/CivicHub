using AutoFixture;
using CivicHub.Application.Common.Extensions;
using NUnit.Framework;

namespace CivicHub.Application.UnitTests.Common.Extensions;

[TestFixture]
public class DateTimeExtensionsTests
{
    private Fixture _fixture;

    [SetUp]
    public void SetUp()
    {
        _fixture = new Fixture();
    }
    
    [Test]
    public void IsNotDefault_When_DateTimeIsDefault_Then_ReturnsFalse()
    {
        // Arrange
        var defaultDateTime = default(DateTime);

        // Act
        var result = defaultDateTime.IsNotDefault();

        // Assert
        Assert.That(result, Is.False);
    }

    [Test]
    public void IsNotDefault_When_DateTimeIsNotDefault_Then_ReturnsTrue()
    {
        // Arrange
        var nonDefaultDateTime = _fixture.Create<DateTime>();

        // Act
        var result = nonDefaultDateTime.IsNotDefault();

        // Assert
        Assert.That(result, Is.True);
    }

    [Test]
    public void IsDefault_When_DateTimeIsDefault_Then_ReturnsTrue()
    {
        // Arrange
        var defaultDateTime = default(DateTime);

        // Act
        var result = defaultDateTime.IsDefault();

        // Assert
        Assert.That(result, Is.True);
    }

    [Test]
    public void IsDefault_When_DateTimeIsNotDefault_Then_ReturnsFalse()
    {
        // Arrange
        var nonDefaultDateTime = _fixture.Create<DateTime>();

        // Act
        var result = nonDefaultDateTime.IsDefault();

        // Assert
        Assert.That(result, Is.False);
    }
}