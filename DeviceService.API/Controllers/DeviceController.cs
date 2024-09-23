using DeviceService.Application.DTOs;
using DeviceService.Application.Features.Device.Commands;
using DeviceService.Application.Features.Device.Queries;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DeviceService.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DeviceController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpPost]
    [Route("create")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> Create([FromBody] CreateDeviceCommand command)
    {
        try
        {
            var deviceId = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id = deviceId }, deviceId);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<DeviceReponseDTO>> GetById(Guid id)
    {
        var device = await _mediator.Send(new GetDeviceByIdQuery(id));

        if (device == null)
            return NotFound();

        return Ok(device);
    }
}
