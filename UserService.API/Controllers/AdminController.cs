using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UserService.Application.Features.Admin.Commands;
using UserService.Domain.Entities;

namespace UserService.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AdminController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpDelete("delete/all")]
    public async Task<ActionResult> DeleteAll()
    {
        var command = new DeleteAllCommand(new List<User>());
        await _mediator.Send(command);
        return Ok();
    }
}
