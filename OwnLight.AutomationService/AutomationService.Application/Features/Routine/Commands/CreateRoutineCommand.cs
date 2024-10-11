using AutomationService.Domain.Enums;
using MediatR;

namespace AutomationService.Application.Features.Routine.Commands;

public class CreateRoutineCommand : IRequest<Guid>
{
    public required string Name { get; set; }
    public TimeSpan ExecutionTime { get; set; }
    public RoutineActionType ActionType { get; set; }
    public Guid? TargetId { get; set; }
    public int? Brightness { get; set; }
    public ActionTarget ActionTarget { get; set; }
}
