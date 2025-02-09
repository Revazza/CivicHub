using CivicHub.Domain.Cities;
using CivicHub.Domain.Persons.Entities.PersonConnections;
using CivicHub.Domain.Persons.Enums;
using CivicHub.Domain.Persons.ValueObjects.PhoneNumbers;

namespace CivicHub.Domain.Persons;

/// <summary>
/// Represents a person with identity and contact details.
/// </summary>
public class Person
{
    /// <summary>
    /// Gets or sets the unique identifier for the person.
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// Gets or sets the first name of the person.
    /// </summary>
    public string FirstName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the last name of the person.
    /// </summary>
    public string LastName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the personal identification number of the person.
    /// </summary>
    public string PersonalNumber { get; set; } = string.Empty;
        
    /// <summary>
    /// Gets or sets the city identifier.
    /// </summary>
    public string CityCode { get; set; }

    /// <summary>
    /// Gets or sets full path to picture
    /// </summary>
    public string PictureFullPath { get; set; }
    
    /// <summary>
    /// Gets or sets the gender of the person.
    /// </summary>
    public Gender Gender { get; set; }

    /// <summary>
    /// Gets or sets the birthdate of the person.
    /// </summary>
    public DateTime BirthDate { get; set; }
    
    /// <summary>
    /// Gets or sets the city details of the person.
    /// </summary>
    public City City { get; set; } = null!;

    /// <summary>
    /// Gets or sets the phone number of the person.
    /// </summary>
    public List<PhoneNumber> PhoneNumbers { get; set; } = null!;
    
    /// <summary>
    /// Gets or sets a list of connections related to the person.
    /// </summary>
    public List<PersonConnection> Connections { get; set; } = [];
    
    /// <summary>
    /// Gets or sets a list of connections related to the person.
    /// </summary>
    public List<PersonConnection> ConnectedTo { get; set; } = [];
}