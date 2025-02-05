namespace CivicHub.Domain.Persons.ValueObjects.PhoneNumbers;

public static class PhoneNumberConstraints
{
    public const int MinCountryCodeLength = 1;
    public const int MaxCountryCodeLength = 3;
    public const int MinAreaCodeLength = 1;
    public const int MaxAreaCodeLength = 5;
    public const int MinNumberLength = 1;
    public const int MaxNumberLength = 10;
}