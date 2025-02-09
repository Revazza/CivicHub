using System.Linq.Expressions;
using CivicHub.Application.Common.Extensions;
using CivicHub.Domain.Persons;
using CivicHub.Domain.Persons.Enums;

namespace CivicHub.Application.Features.Persons.Queries.Common.Builders;

/// <summary>
/// A builder class for constructing LINQ expressions that filter <see cref="Person"/> entities
/// Class supports adding filters using logical operators (can be extended to Or and etc)
/// Method naming convention: {LogicalOperator}{Operation}{PropertyName}
/// </summary>
public class PersonExpressionBuilder
{
    private Expression<Func<Person, bool>> _predicate = p => true;

    public Expression<Func<Person, bool>> Build() => _predicate;

    public PersonExpressionBuilder AndContainsFirstName(string firstName)
    {
        if (firstName.IsNotNullOrEmpty())
        {
            _predicate = _predicate.And(p => p.FirstName.Contains(firstName));
        }

        return this;
    }

    public PersonExpressionBuilder AndContainsLastName(string lastName)
    {
        if (lastName.IsNotNullOrEmpty())
        {
            _predicate = _predicate.And(p => p.LastName.Contains(lastName));
        }

        return this;
    }

    public PersonExpressionBuilder AndContainsPersonalNumber(string personalNumber)
    {
        if (personalNumber.IsNotNullOrEmpty())
        {
            _predicate = _predicate.And(p => p.PersonalNumber.Contains(personalNumber));
        }

        return this;
    }

    public PersonExpressionBuilder AndEqualToGender(Gender gender)
    {
        if (gender != Gender.NotSpecified)
        {
            _predicate = _predicate.And(p => p.Gender == gender);
        }

        return this;
    }

    public PersonExpressionBuilder AndIsInBirthDateRange(DateTime from, DateTime to)
    {
        if (from.IsNotDefault() && to.IsNotDefault())
        {
            _predicate = _predicate.And(p => p.BirthDate >= from && p.BirthDate <= to);
        }
        else if (from.IsNotDefault())
        {
            _predicate = _predicate.And(p => p.BirthDate >= from);
        }
        else if (to.IsNotDefault())
        {
            _predicate = _predicate.And(p => p.BirthDate <= to);
        }

        return this;
    }

    public PersonExpressionBuilder AndContainsCityCode(string cityCode)
    {
        if (cityCode.IsNotNullOrEmpty())
        {
            _predicate = _predicate.And(p => p.CityCode.Contains(cityCode));
        }

        return this;
    }

    public PersonExpressionBuilder AndContainsCityName(string cityName)
    {
        if (cityName.IsNotNullOrEmpty())
        {
            _predicate = _predicate.And(p => p.City.Name.Contains(cityName));
        }

        return this;
    }
}