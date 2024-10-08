using DeviceService.Application.DTOs;
using DeviceService.Application.Features.Device.Commands;
using DeviceService.Application.Features.Device.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DeviceService.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DeviceController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [Authorize]
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<DeviceResponseDTO>> GetById(Guid id)
    {
        var query = new GetDeviceByIdQuery(id);
        var device = await _mediator.Send(query);

        if (device == null)
            return NotFound();

        return Ok(device);
    }

    [Authorize]
    [HttpGet("all")]
    public async Task<ActionResult<PaginatedResultDTO<DeviceResponseDTO>>> GetAll(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10
    )
    {
        if (page <= 0 || pageSize <= 0)
            return BadRequest("Page and PageSize must be greater than zero.");

        var query = new GetAllDevicesQuery(page, pageSize);
        var devices = await _mediator.Send(query);

        if (devices == null)
        {
            return StatusCode(
                StatusCodes.Status500InternalServerError,
                "An error occurred while retrieving devices."
            );
        }

        if (!devices.Items.Any())
            return Ok(devices);

        return Ok(devices);
    }

    [Authorize]
    [HttpPost("create")]
    public async Task<ActionResult> CreateDevice([FromBody] CreateDeviceCommand command)
    {
        var deviceId = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id = deviceId }, deviceId);
    }

    [Authorize]
    [HttpPut("update/device_name/{id:guid}")]
    public async Task<ActionResult> Update(Guid id, [FromBody] UpdateDeviceCommand command)
    {
        command.Id = id;
        await _mediator.Send(command);
        return Ok();
    }

    [Authorize]
    [HttpPut("update/device_room/{id:guid}")]
    public async Task<ActionResult> UpdateDeviceRoom(
        Guid id,
        [FromBody] UpdateDeviceRoomCommand command
    )
    {
        command.Id = id;
        await _mediator.Send(command);
        return Ok();
    }

    [Authorize]
    [HttpPut("update/device_group/{id:guid}")]
    public async Task<ActionResult> UpdateDeviceGroup(
        Guid id,
        [FromBody] UpdateDeviceGroupCommand command
    )
    {
        command.Id = id;
        await _mediator.Send(command);
        return Ok();
    }

    [Authorize]
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> Delete(Guid id)
    {
        await _mediator.Send(new DeleteDeviceCommand(id));
        return NoContent();
    }

    [AllowAnonymous]
    [HttpGet("devices_status")]
    public async Task<ActionResult<IEnumerable<HardwareResponseDTO>>> GetHardwareStatus(
        [FromQuery] Guid[] deviceIds
    )
    {
        var query = new GetHardwareResponseQuery(deviceIds);
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [Authorize]
    [HttpGet("user_devices")]
    public async Task<ActionResult<PaginatedResultDTO<DeviceResponseDTO>>> GetUserDevices(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10
    )
    {
        var query = new GetUserDevicesQuery(page, pageSize);
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [Authorize]
    [HttpGet("user_devices_by_room")]
    public async Task<ActionResult<PaginatedResultDTO<DeviceResponseDTO>>> GetUserDevicesByRoom(
        [FromQuery] Guid roomId,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10
    )
    {
        var query = new GetUserDevicesByRoomIdQuery(roomId, page, pageSize);
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [Authorize]
    [HttpGet("user_devices_by_group")]
    public async Task<ActionResult<PaginatedResultDTO<DeviceResponseDTO>>> GetUserDevicesByGroup(
        [FromQuery] Guid groupId,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10
    )
    {
        var query = new GetUserDevicesByGroupIdQuery(groupId, page, pageSize);
        var result = await _mediator.Send(query);
        return Ok(result);
    }
}
