using AutoFixture;
using CivicHub.Application.Common.Extensions;
using NUnit.Framework;

namespace CivicHub.Application.UnitTests.Common.Extensions;

[TestFixture]
public class EnumerableExtensionsTests
{
    private Fixture _fixture;

    [SetUp]
    public void SetUp()
    {
        _fixture = new Fixture();
    }

    [Test]
    public void IsNotNullOrEmpty_When_EnumerableIsNull_Then_ReturnsFalse()
    {
        // Arrange & Act
        var result = ((IEnumerable<string>)null).IsNotNullOrEmpty();

        // Assert
        Assert.That(result, Is.False);
    }

    [Test]
    public void IsNotNullOrEmpty_When_EnumerableIsEmpty_Then_ReturnsFalse()
    {
        // Arrange
        var emptyEnumerable = Enumerable.Empty<string>();

        // Act
        var result = emptyEnumerable.IsNotNullOrEmpty();

        // Assert
        Assert.That(result, Is.False);
    }

    [Test]
    public void IsNotNullOrEmpty_When_EnumerableHasItems_Then_ReturnsTrue()
    {
        // Arrange
        var nonEmptyEnumerable = _fixture.CreateMany<string>(3);

        // Act
        var result = nonEmptyEnumerable.IsNotNullOrEmpty();

        // Assert
        Assert.That(result, Is.True);
    }

    [Test]
    public void IsEmpty_When_EnumerableIsEmpty_Then_ReturnsTrue()
    {
        // Arrange
        var emptyEnumerable = Enumerable.Empty<string>();

        // Act
        var result = emptyEnumerable.IsEmpty();

        // Assert
        Assert.That(result, Is.True);
    }

    [Test]
    public void IsEmpty_When_EnumerableHasItems_Then_ReturnsFalse()
    {
        // Arrange
        var nonEmptyEnumerable = _fixture.CreateMany<string>(3);

        // Act
        var result = nonEmptyEnumerable.IsEmpty();

        // Assert
        Assert.That(result, Is.False);
    }

    [Test]
    public void IsNotNullOrEmpty_When_WithDifferentTypes_Then_ReturnsTrue()
    {
        // Arrange
        var intList = _fixture.CreateMany<int>(3).ToList();

        // Act
        var result = intList.IsNotNullOrEmpty();
        
        // Assert
        Assert.That(result, Is.True);
    }
}