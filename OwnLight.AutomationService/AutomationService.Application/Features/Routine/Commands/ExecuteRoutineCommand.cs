using MediatR;

namespace AutomationService.Application.Features.Routine.Commands;

public class ExecuteRoutineCommand : IRequest
{
    public TimeSpan CurrentTime { get; set; }
}
