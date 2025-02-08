using CivicHub.Application.Common.Results;
using CivicHub.Application.Features.PersonConnections.Commands.AddConnection;
using CivicHub.Application.Features.PersonConnections.Commands.RemoveConnection;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CivicHub.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PersonConnectionController(IMediator mediator) : ControllerBase
{
    [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status500InternalServerError)]
    [HttpPost]
    public async Task<IActionResult> AddPersonConnection(
        AddConnectionCommand command,
        CancellationToken cancellationToken = default)
    {
        var result = await mediator.Send(command, cancellationToken);
        return Ok(result);
    }

    [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status500InternalServerError)]
    [HttpDelete]
    public async Task<IActionResult> RemovePersonConnection(
        RemoveConnectionCommand command,
        CancellationToken cancellationToken = default)
    {
        var result = await mediator.Send(command, cancellationToken);
        return Ok(result);
    }
}