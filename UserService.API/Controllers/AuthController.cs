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
    public async Task<ActionResult> Login([FromBody] LoginCommand command)
    {
        if (command is null || command.UserName is null or "" || command.Password is null or "")
            return BadRequest();
        else if (
            command.UserName.Length < 3
            || command.Password.Length < 6
            || command.Password.Length > 20
        )
            return BadRequest();
        else
        {
            var result = await _mediator.Send(command);
            var message = result.ToString() ?? "Login failed";
            message = result.ToString() != null ? "Login successful" : message;

            return Ok(message);
        }
    }
}
