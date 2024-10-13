using AutomationService.Application.Contracts.DTOs;
using AutomationService.Application.Features.RoutineLog.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AutomationService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoutineLogController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [Authorize]
        [HttpGet]
        [Route("get/user_logs")]
        public async Task<ActionResult<PaginatedResultDTO<RoutineLogDTO>>> GetRoutineLogsByUserId(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10
        )
        {
            var query = new GetLogsByUserIdQuery(pageNumber, pageSize);
            return Ok(await _mediator.Send(query));
        }
    }
}
