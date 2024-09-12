using MediatR;
using Microsoft.AspNetCore.Mvc;
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
    public async Task<ActionResult<UserResponseDTO>> Create([FromBody] CreateCommand command)
    {
        var result = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<UserResponseDTO>> Update(
        [FromRoute] Guid id,
        [FromBody] UpdateCommand command
    )
    {
        command.Id = id;
        var result = await _mediator.Send(command);
        return Ok(result);
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
