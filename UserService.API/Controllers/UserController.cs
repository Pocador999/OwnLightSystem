using MediatR;
using Microsoft.AspNetCore.Mvc;
using UserService.Application.DTOs;
using UserService.Application.Features.User.Commands;
using UserService.Application.Features.User.Queries;
using UserService.Domain.Entities;

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

    [HttpGet("{userName}")]
    public async Task<ActionResult<UserResponseDTO>> GetByUserName(string userName)
    {
        var query = new GetByUserNameQuery(userName);
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
        if (
            command is null
            || command.Name is null or ""
            || command.UserName is null or ""
            || command.Password is null or ""
        )
            return BadRequest();
        else if (
            command.Name.Length < 3
            || command.UserName.Length < 3
            || command.Password.Length < 6
            || command.Password.Length > 20
            || command.UserName.Length > 30
            || command.Name.Length > 30
        )
        {
            return BadRequest();
        }
        return Ok(await _mediator.Send(command));
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Update([FromRoute] Guid id, [FromBody] UpdateCommand command)
    {
        command.Id = id;

        if (command is null || command.Name is null or "" || command.UserName is null or "")
            return BadRequest();
        else if (
            command.Name.Length < 3
            || command.UserName.Length < 3
            || command.UserName.Length > 30
            || command.Name.Length > 30
        ) return BadRequest();

        await _mediator.Send(command);
        return Ok();
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
