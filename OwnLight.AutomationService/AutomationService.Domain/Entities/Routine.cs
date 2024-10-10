using AutomationService.Domain.Enums;

namespace AutomationService.Domain.Entities;

public class Routine
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid UserId { get; set; }
    public string Name { get; set; } = null!;
    public TimeSpan ExecutionTime { get; set; }
    public RoutineActionType ActionType { get; set; }

    public Guid? TargetId { get; set; }
    public int? Brightness { get; set; }
    public ActionTarget ActionTarget { get; set; }

    public string JwtToken { get; set; } = null!;

    public ICollection<RoutineExecutionLog> ExecutionLogs { get; set; } = [];
}
