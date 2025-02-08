using CivicHub.Application.Common.Responses;

namespace CivicHub.Application.Features.Persons.Queries.GetFullInformation;

public record PersonConnectionResponse(
    Guid Id,
    string ConnectionType,
    ShortPersonResponse Person);