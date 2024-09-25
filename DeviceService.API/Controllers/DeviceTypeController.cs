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
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<DeviceTypeResponseDTO>> GetById(Guid id)
        {
            try
            {
                var deviceType = await _mediator.Send(new GetDeviceTypeByIdQuery(id));
                return Ok(deviceType);
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

        [HttpGet("all")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Create([FromBody] CreateDeviceTypeCommand command)
        {
            try
            {
                await _mediator.Send(command);
                return StatusCode(StatusCodes.Status201Created);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
