using CivicHub.Application.Common.Results;
using CivicHub.Application.Features.Persons.Commands.AddPerson;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CivicHub.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PersonController(IMediator mediator) : ControllerBase
{
    [ProducesResponseType(typeof(Result<AddPersonResponse>), StatusCodes.Status200OK)]
    [HttpPost]
    public async Task<IActionResult> AddPerson(AddPersonCommand command)
    {
        var result = await mediator.Send(command);
        return Ok(result);
    }
}