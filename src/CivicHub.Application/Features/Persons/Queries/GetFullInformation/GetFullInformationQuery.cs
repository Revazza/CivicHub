using CivicHub.Application.Common.Results;
using MediatR;

namespace CivicHub.Application.Features.Persons.Queries.GetFullInformation;

public record GetFullInformationQuery(long PersonId) : IRequest<Result<GetFullInformationResponse>>;