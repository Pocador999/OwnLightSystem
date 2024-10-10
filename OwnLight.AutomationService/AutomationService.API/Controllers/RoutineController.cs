using AutomationService.Application.Features.Routine.Command;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AutomationService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RoutineController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [Authorize]
    [HttpPost]
    [Route("create")]
    public async Task<IActionResult> CreateRoutine([FromBody] CreateRoutineCommand command)
    {
        var routineId = await _mediator.Send(command);
        return Ok(new { Id = routineId });
    }

    [HttpPost]
    [Route("execute/{id}")]
    public async Task<IActionResult> ExecuteRoutine(Guid id)
    {
        await _mediator.Send(new ExecuteRoutineCommand { RoutineId = id });
        return Ok();
    }
}
