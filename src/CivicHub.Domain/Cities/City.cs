namespace CivicHub.Domain.Cities;

/// <summary>
/// Represents city details.
/// </summary>
public class City
{
    /// <summary>
    /// Gets or sets the city code (e.g., "TB" for Tbilisi).
    /// </summary>
    public string Code { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the city name (e.g. Tbilisi).
    /// </summary>
    public string Name { get; set; }
}