namespace CivicHub.Domain.Persons.Entities.PersonConnections;

/// <summary>
/// Represents a connection between two persons.
/// </summary>
public class PersonConnection
{
    /// <summary>
    /// Gets or sets the unique identifier for the connection.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the primary person in the connection.
    /// </summary>
    public long PersonId { get; set; }
    
    /// <summary>
    /// Gets or sets the identifier of the connected person.
    /// </summary>
    public long ConnectedPersonId { get; set; }

    /// <summary>
    /// Gets or sets the type of connection (e.g., friend, colleague, family).
    /// </summary>
    public string ConnectionType { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the primary person in the connection.
    /// </summary>
    public Person Person { get; set; } = null!;

    /// <summary>
    /// Gets or sets the connected person in the connection.
    /// </summary>
    public Person ConnectedPerson { get; set; }= null!;
}