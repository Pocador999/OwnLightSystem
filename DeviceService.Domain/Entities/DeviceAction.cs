using System.ComponentModel.DataAnnotations.Schema;
using DeviceService.Domain.Enums;
using DeviceService.Domain.Primitives;

namespace DeviceService.Domain.Entities;

public class DeviceAction : Entity
{
    [ForeignKey(nameof(UserId))]
    public Guid UserId { get; set; }

    [ForeignKey(nameof(DeviceId))]
    public Guid DeviceId { get; set; }

    public DeviceActions Action { get; set; }

    public DateTime PerformedAt { get; set; } = DateTime.UtcNow;
}
