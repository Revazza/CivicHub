using CivicHub.Domain.Persons;

namespace CivicHub.Domain.Locations;

/// <summary>
/// Represents a geographical location with country and city details.
/// </summary>
public class Location
{
    /// <summary>
    /// Gets or sets the country code of the location (e.g., "GE" for the Georgia).
    /// </summary>
    public string CountryCode { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the city code of the location (e.g., "TB" for Tbilisi).
    /// </summary>
    public string CityCode { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the list of people residing in this location.
    /// </summary>
    public List<Person> Population { get; set; } = [];
}