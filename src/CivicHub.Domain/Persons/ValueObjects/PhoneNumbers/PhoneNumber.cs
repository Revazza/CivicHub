using CivicHub.Domain.Persons.ValueObjects.PhoneNumbers.Enums;

namespace CivicHub.Domain.Persons.ValueObjects.PhoneNumbers;

/// <summary>
/// Represents a phone number with its associated type.
/// </summary>
public record PhoneNumber
{
    /// <summary>
    /// Gets or sets the phone number as a string.
    /// </summary>
    public string Number { get; set; }

    /// <summary>
    /// Gets or sets the type of phone number (e.g., Mobile, Home, Work).
    /// </summary>
    public PhoneType Type { get; set; }
}