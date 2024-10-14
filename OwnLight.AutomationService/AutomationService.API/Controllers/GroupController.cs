using AutomationService.Application.Features.Group.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AutomationService.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class GroupController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [Authorize]
    [HttpPost]
    [Route("create")]
    public async Task<ActionResult<Guid>> CreateGroup([FromBody] CreateGroupCommand command) =>
        Ok(await _mediator.Send(command));

    [Authorize]
    [HttpPut]
    [Route("update/{id}")]
    public async Task<IActionResult> UpdateGroup(Guid id, [FromBody] UpdateGroupCommand command)
    {
        command.Id = id;
        await _mediator.Send(command);
        return Ok("Grupo atualizado com sucesso.");
    }

    [Authorize]
    [HttpDelete]
    [Route("delete/{id}")]
    public async Task<IActionResult> DeleteGroup(Guid id, [FromBody] DeleteGroupCommand command)
    {
        command.Id = id;
        await _mediator.Send(command);
        return Ok("Grupo deletado com sucesso.");
    }

    [Authorize]
    [HttpPost]
    [Route("add-devices/{groupId}")]
    public async Task<IActionResult> AddDevicesToGroup(
        Guid groupId,
        [FromBody] AddDevicesCommand command
    )
    {
        command.GroupId = groupId;
        await _mediator.Send(command);
        return Ok("Dispositivos adicionados ao grupo com sucesso.");
    }
}
