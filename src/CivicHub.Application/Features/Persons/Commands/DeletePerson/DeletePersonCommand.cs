using CivicHub.Application.Common.Results;
using MediatR;

namespace CivicHub.Application.Features.Persons.Commands.DeletePerson;

public record DeletePersonCommand(string PersonalNumber) : IRequest<Result>;