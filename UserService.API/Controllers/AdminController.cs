using MediatR;
using Microsoft.AspNetCore.Mvc;
using UserService.Application.Features.Admin.Commands;

namespace UserService.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AdminController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpDelete("delete/all")]
    public async Task<ActionResult> DeleteAll(
        [FromQuery] Guid adminId,
        [FromQuery] string adminPassword
    )
    {
        var command = new DeleteAllCommand { AdminId = adminId, AdminPassword = adminPassword };
        await _mediator.Send(command);
        return NoContent();
    }
}
