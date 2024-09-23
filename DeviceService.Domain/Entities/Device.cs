using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DeviceService.Domain.Primitives;

namespace DeviceService.Domain.Entities;

public class Device : Entity
{
    [Key]
    public Guid DeviceId { get; protected set; } = Guid.NewGuid();

    [ForeignKey(nameof(DeviceTypeId))]
    public Guid DeviceTypeId { get; set; }

    [ForeignKey(nameof(UserId))]
    public Guid UserId { get; set; }

    [ForeignKey(nameof(RoomId))]
    public Guid RoomId { get; set; }

    [Range(3, 50)]
    public required string Name { get; set; }

    [DefaultValue(false)]
    public bool? IsDimmable { get; set; }
}
