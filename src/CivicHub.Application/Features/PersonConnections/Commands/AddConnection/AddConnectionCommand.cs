using CivicHub.Application.Common.Results;
using MediatR;

namespace CivicHub.Application.Features.PersonConnections.Commands.AddConnection;

public record AddConnectionCommand(long PersonId, long OtherPersonId, string ConnectionType) : IRequest<Result>;