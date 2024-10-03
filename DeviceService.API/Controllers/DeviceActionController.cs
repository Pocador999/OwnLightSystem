using DeviceService.Application.Features.DeviceAction.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DeviceService.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DeviceActionController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpPost("control/{deviceId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> ControlDeviceAsync(
        Guid deviceId,
        [FromBody] ControlDeviceCommand command
    )
    {
        try
        {
            command.DeviceId = deviceId;
            var result = await _mediator.Send(command);
            string message = "Device controlled successfully.";
            return Ok(message);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    // [HttpPost("switch/{deviceId}")]
    // [ProducesResponseType(StatusCodes.Status200OK)]
    // [ProducesResponseType(StatusCodes.Status400BadRequest)]
    // [ProducesResponseType(StatusCodes.Status404NotFound)]
    // [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    // public async Task<ActionResult> SwitchAsync(Guid deviceId, [FromBody] SwitchCommand command)
    // {
    //     command.DeviceId = deviceId;
    //     var result = await _mediator.Send(command);
    //     return Ok(result);
    // }

    // [HttpPost("brightness/{deviceId}")]
    // [ProducesResponseType(StatusCodes.Status200OK)]
    // [ProducesResponseType(StatusCodes.Status400BadRequest)]
    // [ProducesResponseType(StatusCodes.Status404NotFound)]
    // [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    // public async Task<ActionResult> ControlBrightnessAsync(
    //     Guid deviceId,
    //     [FromBody] ControlBrightnessCommand command
    // )
    // {
    //     command.DeviceId = deviceId;
    //     var result = await _mediator.Send(command);
    //     return Ok(result);
    // }
}
