using AutoFixture;
using CivicHub.Application.Common.Extensions;
using NUnit.Framework;

namespace CivicHub.Application.UnitTests.Common.Extensions;

[TestFixture]
public class StringExtensionsTests
{
    private Fixture _fixture;

    [SetUp]
    public void SetUp()
    {
        _fixture = new Fixture();
    }

    [TestCase(null, null)]
    [TestCase("", "")]
    [TestCase("a", "A")]
    [TestCase("name", "Name")]
    [TestCase("1name", "1name")] // This is expected behaviour
    [TestCase("n1ame", "N1ame")]
    [TestCase("Name", "Name")]
    [TestCase("123", "123")]
    public void Capitalize_When_InputIsPassed_Then_ReturnsExpected(string input, string expected)
    {
        // Act
        var result = input.Capitalize();

        // Assert
        Assert.That(result, Is.EqualTo(expected));
    }

    [TestCase("", true)]
    [TestCase(null, true)]
    [TestCase("value", false)]
    public void IsNullOrEmpty_When_InputIsPassed_Then_ReturnsExpected(string input, bool expected)
    {
        // Act
        var result = input.IsNullOrEmpty();

        // Assert
        Assert.That(result, Is.EqualTo(expected));
    }

    [TestCase("", false)]
    [TestCase(null, false)]
    [TestCase("value", true)]
    public void IsNotNullOrEmpty_When_InputIsPassed_Then_ReturnsExpected(string input, bool expected)
    {
        // Act
        var result = input.IsNotNullOrEmpty();

        // Assert
        Assert.That(result, Is.EqualTo(expected));
    }
}