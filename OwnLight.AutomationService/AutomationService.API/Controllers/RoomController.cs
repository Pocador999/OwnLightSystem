using AutomationService.Application.Features.Room.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AutomationService.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RoomController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [Authorize]
    [HttpPost("create")]
    public async Task<IActionResult> CreateRoom([FromBody] CreateRoomCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [Authorize]
    [HttpPut("update/{id}")]
    public async Task<IActionResult> UpdateRoom(Guid id, [FromBody] UpdateRoomCommand command)
    {
        command.Id = id;
        await _mediator.Send(command);
        return Ok($"Cômodo '{command.Name}' atualizado com sucesso.");
    }

    [Authorize]
    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> DeleteRoom(Guid id)
    {
        await _mediator.Send(new DeleteRoomCommand { Id = id });
        return Ok("Cômodo removido com sucesso.");
    }
}
