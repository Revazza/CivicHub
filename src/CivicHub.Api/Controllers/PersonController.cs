using CivicHub.Application.Common.Results;
using CivicHub.Application.Features.Persons.Commands.AddPerson;
using CivicHub.Application.Features.Persons.Commands.DeletePerson;
using CivicHub.Application.Features.Persons.Commands.UpdatePerson;
using CivicHub.Application.Features.Persons.Queries.GetFullInformation;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CivicHub.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PersonController(IMediator mediator) : ControllerBase
{
    [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status500InternalServerError)]
    [HttpPost]
    public async Task<IActionResult> AddPerson(AddPersonCommand command, CancellationToken cancellationToken = default)
    {
        var result = await mediator.Send(command, cancellationToken);
        return Ok(result);
    }

    [ProducesResponseType(typeof(Result<GetFullInformationResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status500InternalServerError)]
    [HttpGet("full")]
    public async Task<IActionResult> GetPerson(
        [FromBody] GetFullInformationQuery query,
        CancellationToken cancellationToken = default)
    {
        var result = await mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status500InternalServerError)]
    [HttpPut]
    public async Task<IActionResult> UpdatePerson(
        UpdatePersonCommand command,
        CancellationToken cancellationToken = default)
    {
        var result = await mediator.Send(command, cancellationToken);
        return Ok(result);
    }

    [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status500InternalServerError)]
    [HttpDelete]
    public async Task<IActionResult> DeletePerson(
        DeletePersonCommand command,
        CancellationToken cancellationToken = default)
    {
        var result = await mediator.Send(command, cancellationToken);
        return Ok(result);
    }
}