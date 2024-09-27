using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserService.Application.DTOs;
using UserService.Application.Features.User.Commands;
using UserService.Application.Features.User.Commands.Update;
using UserService.Application.Features.User.Queries;

namespace UserService.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [Authorize]
    [HttpGet("id")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UserResponseDTO>> GetById(Guid id)
    {
        var query = new GetByIdQuery(id);
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [Authorize]
    [HttpGet("{username}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UserResponseDTO>> GetByUsername(string username)
    {
        var query = new GetByUsernameQuery(username);
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [Authorize]
    [HttpGet("all")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<List<UserResponseDTO>>> GetAll(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10
    )
    {
        var query = new GetAllQuery(page, pageSize);
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [AllowAnonymous]
    [HttpPost]
    [Route("create")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult> Create([FromBody] CreateCommand command)
    {
        var result = await _mediator.Send(command);

        if (int.TryParse(result.StatusCode, out var statusCode))
        {
            if (statusCode == StatusCodes.Status201Created)
                return CreatedAtAction(nameof(GetById), result);
            if (statusCode == StatusCodes.Status409Conflict)
                return Conflict(result);
        }

        return BadRequest(result);
    }

    [Authorize]
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult> Update([FromRoute] Guid id, [FromBody] UpdateCommand command)
    {
        command.Id = id;
        var result = await _mediator.Send(command);

        if (result.StatusCode == StatusCodes.Status200OK.ToString())
            return Ok(result);

        if (result.StatusCode == StatusCodes.Status404NotFound.ToString())
            return NotFound(result);

        return BadRequest(result);
    }

    [Authorize]
    [HttpPut("password/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult> ChangePassword(
        [FromRoute] Guid id,
        [FromBody] UpdatePasswordCommand command
    )
    {
        command.Id = id;
        var result = await _mediator.Send(command);

        if (result.StatusCode == StatusCodes.Status200OK.ToString())
            return Ok(result);

        if (result.StatusCode == StatusCodes.Status404NotFound.ToString())
            return NotFound(result);

        return BadRequest(result);
    }

    [Authorize]
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult> Delete(Guid id)
    {
        var command = new DeleteCommand(id);
        var result = await _mediator.Send(command);

        if (result.StatusCode == StatusCodes.Status200OK.ToString())
            return Ok(result);

        return NotFound(result);
    }

    [Authorize]
    [HttpPut("email/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult> UpdateEmail(
        [FromRoute] Guid id,
        [FromBody] UpdateEmailCommand command
    )
    {
        command.Id = id;
        var result = await _mediator.Send(command);

        if (result.StatusCode == StatusCodes.Status200OK.ToString())
            return Ok(result);
        if (result.StatusCode == StatusCodes.Status404NotFound.ToString())
            return NotFound(result);

        return BadRequest(result);
    }

    [Authorize]
    [HttpPut("username/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult> UpdateUsername(
        [FromRoute] Guid id,
        [FromBody] UpdateUsernameCommand command
    )
    {
        command.Id = id;
        var result = await _mediator.Send(command);

        if (result.StatusCode == StatusCodes.Status200OK.ToString())
            return Ok(result);
        if (result.StatusCode == StatusCodes.Status404NotFound.ToString())
            return NotFound(result);

        return BadRequest(result);
    }
}
