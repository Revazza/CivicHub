using AutoFixture;
using AutoFixture.Dsl;
using CivicHub.Application.Features.Persons.Queries.DetailedSearchPerson;
using CivicHub.Application.IntegrationTestsCommon;
using CivicHub.Application.IntegrationTestsCommon.Extensions;
using CivicHub.Domain.Persons;
using NUnit.Framework;

namespace CivicHub.Application.IntegrationTests.Features.Persons.Queries.DetailedSearchPerson;

public class DetailedSearchPersonQueryHandlerTests : IntegrationTestBase
{
    private IPostprocessComposer<Person> _personComposer;
    private const int PageSize = 10;
    private const int PageNumber = 1;

    [SetUp]
    public void SetUp()
    {
        _personComposer = Fixture.GetPersonComposer();
    }
    
    [Test]
    public async Task When_MatchingPersonInfoIsPassed_Then_ReturnFoundPerson()
    {
        // Arrange
        var persons = _personComposer.Without(x => x.Id).CreateMany(5).ToList();

        await Context.AddRangeAsync(persons);
        await Context.SaveChangesAsync();

        var personWeAreLookingFor = persons.First();

        // Act

        var query = new DetailedSearchPersonQuery(
            PageSize,
            PageNumber,
            personWeAreLookingFor.FirstName,
            personWeAreLookingFor.LastName,
            personWeAreLookingFor.PersonalNumber,
            personWeAreLookingFor.City.Name,
            personWeAreLookingFor.CityCode,
            personWeAreLookingFor.Gender,
            personWeAreLookingFor.BirthDate.AddYears(-1),
            personWeAreLookingFor.BirthDate.AddYears(1));

        var result = await Sender.Send(query);

        // Assert #1
        Assert.Multiple(() =>
        {
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(result.Value.TotalCount, Is.EqualTo(1));
        });

        // Assert #2
        var foundPerson = result.Value.Items.Single();
        Assert.Multiple(() =>
        {
            Assert.That(foundPerson.Id, Is.EqualTo(personWeAreLookingFor.Id));
            Assert.That(foundPerson.PersonalNumber, Is.EqualTo(personWeAreLookingFor.PersonalNumber));
            Assert.That(foundPerson.BirthDate, Is.EqualTo(personWeAreLookingFor.BirthDate));
            Assert.That(foundPerson.Gender, Is.EqualTo(personWeAreLookingFor.Gender));
            Assert.That(foundPerson.Gender, Is.EqualTo(personWeAreLookingFor.Gender));
            Assert.That(foundPerson.FullName, Is.EqualTo(ConstructFullName(personWeAreLookingFor)));
        });
    }

    private static string ConstructFullName(Person person)
        => $"{person.FirstName} {person.LastName}";
}