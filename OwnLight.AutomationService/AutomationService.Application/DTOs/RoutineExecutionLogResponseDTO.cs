namespace AutomationService.Application.DTOs;

public class RoutineLogResponseDTO
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid DeviceId { get; set; }
    public Guid RoutineId { get; set; }

    public required string ActionTarget { get; set; }
    public required string ActionStatus { get; set; }
    public required string ActionType { get; set; }

    public string ErrorMessage { get; set; } = string.Empty;
    public DateTime ExecutedAt { get; set; }
}
