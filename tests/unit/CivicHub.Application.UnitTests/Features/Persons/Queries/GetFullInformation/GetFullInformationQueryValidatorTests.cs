using AutoFixture;
using CivicHub.Application.Features.Persons.Queries.GetFullInformation;
using FluentValidation.TestHelper;
using NUnit.Framework;

namespace CivicHub.Application.UnitTests.Features.Persons.Queries.GetFullInformation;

[TestFixture]
public class GetFullInformationQueryValidatorTests
{
    private Fixture _fixture;
    private GetFullInformationQueryValidator _validator;

    [SetUp]
    public void SetUp()
    {
        _fixture = new Fixture();
        _validator = new GetFullInformationQueryValidator();
    }

    [TestCase(0)]
    [TestCase(-1)]
    public void When_PersonIdIsLessThanOrEqualToZero_Then_ValidationFails(int invalidPersonId)
    {
        var query = _fixture.Build<GetFullInformationQuery>().With(x => x.PersonId, invalidPersonId).Create();

        var result = _validator.TestValidate(query);

        result.ShouldHaveValidationErrorFor(x => x.PersonId);
    }

    [Test]
    public void When_PersonIsGreaterThanZero_Then_ValidationFails()
    {
        var query = _fixture.Build<GetFullInformationQuery>()
            .With(x => x.PersonId, 1).Create();

        var result = _validator.TestValidate(query);

        result.ShouldNotHaveValidationErrorFor(x => x.PersonId);
    }
}