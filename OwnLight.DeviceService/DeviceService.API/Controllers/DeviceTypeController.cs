using DeviceService.Application.DTOs;
using DeviceService.Application.Features.DeviceType.Commands;
using DeviceService.Application.Features.DeviceType.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DeviceService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeviceTypeController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpGet("{id}: guid")]
        public async Task<ActionResult<DeviceTypeResponseDTO>> GetById(Guid id)
        {
            var query = new GetDeviceTypeByIdQuery(id);
            var deviceType = await _mediator.Send(query);

            if (deviceType == null)
                return NotFound();

            return Ok(deviceType);
        }

        [HttpGet("all")]
        public async Task<ActionResult<PaginatedResultDTO<DeviceTypeResponseDTO>>> GetAll(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10
        )
        {
            var query = new GetAllDeviceTypesQuery(page, pageSize);
            var deviceTypes = await _mediator.Send(query);

            if (!deviceTypes.Items.Any())
                return NotFound();

            return Ok(deviceTypes);
        }

        [HttpPost("create")]
        public async Task<ActionResult> Create([FromBody] CreateDeviceTypeCommand command)
        {
            await _mediator.Send(command);
            return StatusCode(201);
        }

        [HttpPut("update")]
        public async Task<ActionResult> Update(Guid id, [FromBody] UpdateDeviceTypeCommand command)
        {
            command.Id = id;
            await _mediator.Send(command);
            return Ok();
        }

        [HttpDelete("delete/{id:guid}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            var command = new DeleteDeviceTypeCommand { Id = id };
            await _mediator.Send(command);
            return Ok();
        }
    }
}
