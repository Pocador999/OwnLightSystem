using MediatR;

namespace AutomationService.Application.Features.Routine.Command;

public class ExecuteRoutineCommand : IRequest
{
    public Guid RoutineId { get; set; }
}
