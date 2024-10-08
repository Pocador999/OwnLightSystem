using DeviceService.Application.Features.DeviceAction.Commands;
using DeviceService.Application.Features.DeviceAction.Queries;
using DeviceService.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DeviceService.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DeviceActionController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [Authorize]
    [HttpPost("control/status/{deviceId}")]
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
            return Ok("Dispositivo controlado com sucesso.");
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

    [Authorize]
    [HttpPost("control/switch/{deviceId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> SwitchDeviceAsync(Guid deviceId)
    {
        try
        {
            var command = new SwitchDeviceCommand { DeviceId = deviceId };
            var result = await _mediator.Send(command);
            return Ok("Dispositivo controlado com sucesso.");
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

    [Authorize]
    [HttpPost("control/dim/{deviceId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> DimmerizeDeviceAsync(
        Guid deviceId,
        [FromBody] DimmerizeDeviceCommand command
    )
    {
        try
        {
            command.DeviceId = deviceId;
            var result = await _mediator.Send(command);
            return Ok("Dispositivo controlado com sucesso.");
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

    [Authorize]
    [HttpPost("control/room/{roomId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> ControlUserDevicesByRoomIdAsync(
        Guid roomId,
        [FromBody] ControlRoomCommand command
    )
    {
        try
        {
            command.RoomId = roomId;
            var result = await _mediator.Send(command);
            return Ok("Dispositivos controlados com sucesso.");
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

    [Authorize]
    [HttpPost("dim/room/{roomId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> DimmerizeUserDevicesByRoomIdAsync(
        Guid roomId,
        [FromBody] DimmerizeRoomCommand command
    )
    {
        try
        {
            command.RoomId = roomId;
            var result = await _mediator.Send(command);
            return Ok("Dispositivos controlados com sucesso.");
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

    [Authorize]
    [HttpPost("control/group/{groupId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> ControlUserDevicesByGroupIdAsync(
        Guid groupId,
        [FromBody] ControlGroupCommand command
    )
    {
        try
        {
            command.GroupId = groupId;
            var result = await _mediator.Send(command);
            return Ok("Dispositivos controlados com sucesso.");
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

    [Authorize]
    [HttpPost("dim/group/{groupId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> DimmerizeUserDevicesByGroupIdAsync(
        Guid groupId,
        [FromBody] DimmerizeGroupCommand command
    )
    {
        try
        {
            command.GroupId = groupId;
            var result = await _mediator.Send(command);
            return Ok("Dispositivos controlados com sucesso.");
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

    [Authorize]
    [HttpGet("user_actions")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> GetUserActionsAsync(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10
    )
    {
        try
        {
            var query = new GetActionsByUserIdQuery
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
            };
            var result = await _mediator.Send(query);
            return Ok(result);
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

    [Authorize]
    [HttpGet("device_actions/{deviceId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> GetDeviceActionsAsync(
        Guid deviceId,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10
    )
    {
        try
        {
            var query = new GetActionsByDeviceIdQuery
            {
                DeviceId = deviceId,
                PageNumber = pageNumber,
                PageSize = pageSize,
            };
            var result = await _mediator.Send(query);
            return Ok(result);
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

    [Authorize]
    [HttpGet("user_actions/status/{status}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> GetUserActionsByStatusAsync(
        ActionStatus status,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10
    )
    {
        try
        {
            var query = new GetUserActionsByStatusQuery
            {
                Status = status.ToString(),
                PageNumber = pageNumber,
                PageSize = pageSize,
            };
            var result = await _mediator.Send(query);
            return Ok(result);
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

    [Authorize]
    [HttpGet("user_actions/type/{actionType}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> GetUserActionsByTypeAsync(
        DeviceActions actionType,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10
    )
    {
        try
        {
            var query = new GetUserActionsByTypeQuery
            {
                Action = actionType.ToString(),
                PageNumber = pageNumber,
                PageSize = pageSize,
            };
            var result = await _mediator.Send(query);
            return Ok(result);
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

    [Authorize]
    [HttpGet("all/actions/type/{actionType}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> GetActionsByTypeAsync(
        DeviceActions actionType,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10
    )
    {
        try
        {
            var query = new GetActionsByTypeQuery
            {
                Action = actionType.ToString(),
                PageNumber = pageNumber,
                PageSize = pageSize,
            };
            var result = await _mediator.Send(query);
            return Ok(result);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    [Authorize]
    [HttpGet("all/actions/status/{status}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> GetActionsByStatusAsync(
        ActionStatus status,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10
    )
    {
        try
        {
            var query = new GetActionsByStatusQuery
            {
                Status = status.ToString(),
                PageNumber = pageNumber,
                PageSize = pageSize,
            };
            var result = await _mediator.Send(query);
            return Ok(result);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }
}
