using System.Text.RegularExpressions;
using AutoFixture;
using AutoFixture.Dsl;
using CivicHub.Domain.Cities;
using CivicHub.Domain.Persons;
using CivicHub.Domain.Persons.Enums;
using CivicHub.Domain.Persons.ValueObjects.PhoneNumbers;
using CivicHub.Domain.Persons.ValueObjects.PhoneNumbers.Enums;

namespace CivicHub.Application.IntegrationTestsCommon.Extensions;

public static class PersonExtensions
{
    private static readonly IPostprocessComposer<Person> PersonComposer;
    private static readonly IPostprocessComposer<City> CityComposer;

    static PersonExtensions()
    {
        var fixture = new Fixture();

        CityComposer = fixture
            .Build<City>()
            .With(x => x.Code, fixture.GenerateCityCode)
            .With(x => x.Name, fixture.GenerateCityName);

        var city = fixture.GenerateCity();

        PersonComposer = fixture
            .Build<Person>()
            .With(x => x.FirstName, fixture.GenerateFirstName)
            .With(x => x.LastName, fixture.GenerateLastName)
            .With(x => x.PersonalNumber, fixture.GeneratePersonalNumber)
            .With(x => x.CityCode, city.Code)
            .With(x => x.Gender, Gender.Male)
            .With(x => x.City, city)
            .With(x => x.BirthDate, fixture.GenerateBirthDate)
            .With(x => x.PhoneNumbers, [fixture.GeneratePhoneNumber()])
            .Without(x => x.Connections)
            .Without(x => x.ConnectedTo);
    }

    public static IPostprocessComposer<Person> GetPersonComposer(this Fixture fixture) => PersonComposer;

    public static IPostprocessComposer<City> GetCityComposer(this Fixture fixture) => CityComposer;

    public static string GenerateFirstName(this Fixture fixture) =>
        fixture.GenerateString(PersonConstraints.FirstNameMaxLength);

    public static DateTime GenerateBirthDate(this Fixture fixture) =>
        fixture.Create<DateTime>()
            .Clamp(DateTime.Today.AddYears(-100), DateTime.Today.AddYears(-19));

    public static string GenerateLastName(this Fixture fixture) =>
        fixture.GenerateString(PersonConstraints.LastNameMaxLength);

    public static string GenerateCityName(this Fixture fixture) =>
        fixture.GenerateString(CityConstraints.MaxCityNameLength);

    public static string GenerateCityCode(this Fixture fixture) =>
        fixture.GenerateString(CityConstraints.MaxCityCodeLength);

    public static string GenerateNumber(this Fixture fixture) =>
        fixture.GenerateNumberString(PhoneNumberConstraints.MaxNumberLength);

    public static PhoneType GeneratePhoneType(this Fixture fixture) =>
        fixture.Create<PhoneType>();

    public static PhoneNumber GeneratePhoneNumber(this Fixture fixture) =>
        fixture
            .Build<PhoneNumber>()
            .With(x => x.Number, fixture.GenerateNumber)
            .With(x => x.Type, fixture.GeneratePhoneType)
            .Create();

    public static string GeneratePersonalNumber(this Fixture fixture) =>
        fixture.GenerateNumberString(PersonConstraints.PersonalNumberLength);

    public static City GenerateCity(this Fixture fixture)
        => CityComposer.Create();

    public static string GenerateString(this Fixture fixture, int maxLength)
        => string.Join("", fixture.Create<string>().Where(char.IsLetter).Take(maxLength));

    public static string GenerateNumberString(this Fixture fixture, int maxLength)
        => string.Join("", fixture.Create<string>().Where(char.IsDigit).Take(maxLength));

    private static DateTime Clamp(this DateTime value, DateTime min, DateTime max)
        => value < min ? min : (value > max ? max : value);
}