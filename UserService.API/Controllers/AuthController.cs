using MediatR;
using Microsoft.AspNetCore.Mvc;
using UserService.Application.Features.Authentication.Command;

namespace UserService.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpPost]
    [Route("login")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Login([FromBody] LoginCommand command)
    {
        var result = await _mediator.Send(command);

        if (result.StatusCode == StatusCodes.Status200OK.ToString())
            return Ok(result);
        else if (result.StatusCode == StatusCodes.Status401Unauthorized.ToString())
            return Unauthorized(result);
        else if (result.StatusCode == StatusCodes.Status404NotFound.ToString())
            return NotFound(result);
        else
            return BadRequest(result);
    }

    [HttpPost]
    [Route("logout/{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Logout([FromRoute] Guid id)
    {
        var result = await _mediator.Send(new LogoutCommand(id));
        if (result.StatusCode == StatusCodes.Status200OK.ToString())
            return Ok(result);
        else
            return BadRequest(result);
    }
}
