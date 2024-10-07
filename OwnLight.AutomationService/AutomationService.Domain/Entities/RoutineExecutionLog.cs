using AutomationService.Domain.Enums;

namespace AutomationService.Domain.Entities;

public class RoutineExecutionLog
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }

    public Guid RoutineId { get; set; }
    public Routine Routine { get; set; } = null!;

    public DateTime ExecutedAt { get; set; } = DateTime.UtcNow;
    public ActionStatus Status { get; set; }
    public string ErrorMessage { get; set; } = string.Empty;
}
