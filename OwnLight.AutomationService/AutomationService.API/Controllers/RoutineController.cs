using AutomationService.Application.Features.Routine.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AutomationService.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RoutineController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [Authorize]
    [HttpPost("create")]
    public async Task<IActionResult> CreateRoutine([FromBody] CreateRoutineCommand command)
    {
        await _mediator.Send(command);
        return Ok(new { Message = "Rotina cadastrada com sucesso!" });
    }
}
