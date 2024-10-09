using AutomationService.Application.DTOs;
using MediatR;

namespace AutomationService.Application.Features.Routine.Queries;

public class GetRoutineByIdQuery : IRequest<RoutineResponseDTO>
{
    public Guid Id { get; set; }
}
