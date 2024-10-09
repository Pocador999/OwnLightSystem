using AutomationService.Domain.Enums;

namespace AutomationService.Domain.Entities;

public class Routine
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Name { get; set; } = null!;
    public TimeSpan ExecutionTime { get; set; }
    public RecurrencePattern RecurrencePattern { get; set; } = RecurrencePattern.Daily;
    public RoutineActionType ActionType { get; set; }

    public Guid? DeviceId { get; set; }
    public DeviceStatus DeviceStatus { get; set; }
    public int? Brightness { get; set; }
    public ActionTarget ActionTarget { get; set; }

    public ICollection<RoutineExecutionLog> ExecutionLogs { get; set; } = [];
}
