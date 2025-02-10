using CivicHub.Application.Common.Responses;
using CivicHub.Application.Common.Results;
using CivicHub.Application.Features.Persons.Commands.AddPerson;
using CivicHub.Application.Features.Persons.Commands.DeletePerson;
using CivicHub.Application.Features.Persons.Commands.UpdatePerson;
using CivicHub.Application.Features.Persons.Commands.UploadPersonPicture;
using CivicHub.Application.Features.Persons.Queries.DetailedSearchPerson;
using CivicHub.Application.Features.Persons.Queries.GetFullInformation;
using CivicHub.Application.Features.Persons.Queries.GetReport;
using CivicHub.Application.Features.Persons.Queries.SimpleSearchPerson;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CivicHub.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PersonController(ISender mediator) : ControllerBase
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
        [FromQuery] GetFullInformationQuery query,
        CancellationToken cancellationToken = default)
    {
        var result = await mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    [ProducesResponseType(typeof(Result<PaginatedResult<ShortPersonResponse>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status500InternalServerError)]
    [HttpGet("search/simple")]
    public async Task<IActionResult> SearchPerson(
        [FromQuery] SimpleSearchPersonQuery query,
        CancellationToken cancellationToken = default)
    {
        var result = await mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    [ProducesResponseType(typeof(Result<PaginatedResult<ShortPersonResponse>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status500InternalServerError)]
    [HttpGet("search/detailed")]
    public async Task<IActionResult> SearchPerson(
        [FromQuery] DetailedSearchPersonQuery query,
        CancellationToken cancellationToken = default)
    {
        var result = await mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    [ProducesResponseType(typeof(Result<ReportResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status500InternalServerError)]
    [HttpGet("report")]
    public async Task<IActionResult> GetReport(
        [FromQuery] GetReportQuery query,
        CancellationToken cancellationToken = default)
    {
        var result = await mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    [ProducesResponseType(typeof(Result<string>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status500InternalServerError)]
    [HttpPost("picture")]
    public async Task<IActionResult> UploadPersonPicture(
        long personId,
        IFormFile picture,
        CancellationToken cancellationToken = default)
    {
        var result = await mediator.Send(new UploadPersonPictureCommand(personId, picture), cancellationToken);
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