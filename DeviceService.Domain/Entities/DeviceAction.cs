using System.ComponentModel.DataAnnotations.Schema;
using DeviceService.Domain.Primitives;

namespace DeviceService.Domain.Entities;

public class DeviceAction : Entity
{
    public required string ActionType { get; set; }
    public DateTime PerformedAt { get; set; } = DateTime.UtcNow;
}
