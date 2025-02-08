using CivicHub.Application.Common.Results;
using MediatR;

namespace CivicHub.Application.Features.PersonConnections.Commands.RemoveConnection;

public record RemoveConnectionCommand(Guid ConnectionId) : IRequest<Result>;