using AutomationService.Domain.Enums;

namespace AutomationService.Domain.Entities;

public class Routine
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Name { get; set; } = null!;
    public TimeSpan ExecutionTime { get; set; }
    public string RecurrencePattern { get; set; } = null!;
    public RoutineActionType ActionType { get; set; }

    // Parâmetros para setar a ação
    public Guid DeviceId { get; set; }
    public DeviceStatus? Status { get; set; }
    public int? Brightness { get; set; }

    public ICollection<RoutineExecutionLog> ExecutionLogs { get; set; } = [];
}
