using System.Text.Json.Serialization;
using AutomationService.Domain.Enums;
using MediatR;

namespace AutomationService.Application.Features.Routine.Commands;

public class CreateRoutineCommand : IRequest
{
    [JsonIgnore]
    public Guid UserId { get; set; }
    public string Name { get; set; } = null!;
    public TimeSpan ExecutionTime { get; set; }
    public ActionTarget ActionTarget { get; set; }
    public Guid TargetId { get; set; }
    public RoutineActionType ActionType { get; set; }
    public int? Brightness { get; set; }
}
