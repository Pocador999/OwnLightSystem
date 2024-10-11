using AutomationService.Application.Features.Routine.Commands;
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
        await _mediator.Send(command);
        return Ok("Rotina criada com sucesso.");
    }
}
