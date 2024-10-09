namespace AutomationService.Application.DTOs;

public class RoutineResponseDTO
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public required string Name { get; set; }
    public TimeSpan ExecutionTime { get; set; }
    public required string RecurrencePattern { get; set; }
    public required string ActionType { get; set; }

    public Guid? DeviceId { get; set; }
    public required string DeviceStatus { get; set; }
    public int? Brightness { get; set; }
    public required string ActionTarget { get; set; }

    public ICollection<RoutineLogResponseDTO>? ExecutionLogs { get; set; }
}
