using MediatR;
using Microsoft.AspNetCore.Mvc;
using UserService.Application.DTOs;
using UserService.Application.Features.User.Commands;
using UserService.Application.Features.User.Queries;

namespace UserService.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IMediator _mediator;

    public UserController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("id")]
    public async Task<ActionResult<UserResponseDTO>> GetById(Guid id)
    {
        var query = new GetByIdQuery(id);
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
        [FromBody] UpdateUserCommand command
    )
    {
        command.Id = id;

        var result = await _mediator.Send(command);
        return Ok(result);
    }
}
