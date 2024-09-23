using System.ComponentModel.DataAnnotations;
using DeviceService.Domain.Primitives;

namespace DeviceService.Domain.Entities;

public class DeviceAction : Entity
{
    [Key]
    public required string ActionType { get; set; }
    public DateTime PerformedAt { get; set; } = DateTime.UtcNow;
}
