using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using UserService.Application.Common.Messages;
using UserService.Application.DTOs;
using UserService.Application.Features.User.Commands;
using UserService.Application.Features.User.Queries;

namespace UserService.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpGet("id")]
    public async Task<ActionResult<UserResponseDTO>> GetById(Guid id)
    {
        var query = new GetByIdQuery(id);
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("{username}")]
    public async Task<ActionResult<UserResponseDTO>> GetByUsername(string username)
    {
        var query = new GetByUsernameQuery(username);
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("all")]
    public async Task<ActionResult<List<UserResponseDTO>>> GetAll(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10
    )
    {
        var query = new GetAllQuery(page, pageSize);
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpPost]
    [Route("create")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> Create([FromBody] CreateCommand command)
    {
        var result = await _mediator.Send(command);

        if (result.StatusCode == StatusCodes.Status200OK.ToString())
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Update([FromRoute] Guid id, [FromBody] UpdateCommand command)
    {
        command.Id = id;
        var successMessage = new
        {
            Message = "User updated successfully",
            StatusCode = StatusCodes.Status200OK,
        };
        var errorMessage = new
        {
            Title = "Bad Request",
            Message = "name and username cannot be null or empty",
            Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
            StatusCode = StatusCodes.Status404NotFound,
            TraceId = Guid.NewGuid(),
        };

        if (command is null || command.Name is null or "" || command.UserName is null or "")
            return BadRequest(errorMessage);
        else if (
            command.Name.Length < 3
            || command.UserName.Length < 3
            || command.UserName.Length > 30
            || command.Name.Length > 30
        )
            return BadRequest(errorMessage);

        await _mediator.Send(command);
        return Ok(successMessage);
    }

    [HttpPut("password/{id}")]
    public async Task<ActionResult> ChangePassword(
        [FromRoute] Guid id,
        [FromBody] UpdatePasswordCommand command
    )
    {
        command.Id = id;
        await _mediator.Send(command);
        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(Guid id)
    {
        var command = new DeleteCommand { Id = id };
        await _mediator.Send(command);
        return Ok();
    }
}
