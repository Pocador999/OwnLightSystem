using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DeviceService.Domain.Primitives;

namespace DeviceService.Domain.Entities;

public class Device : Entity
{
    [ForeignKey(nameof(DeviceType))]
    public Guid DeviceTypeId { get; set; }
    public virtual required DeviceType DeviceType { get; set; }

    [ForeignKey(nameof(UserId))]
    public Guid UserId { get; set; }

    [ForeignKey(nameof(RoomId))]
    public Guid RoomId { get; set; }

    [ForeignKey(nameof(GroupId))]
    public Guid? GroupId { get; set; }

    [Range(3, 50)]
    public required string Name { get; set; }

    [DefaultValue(false)]
    public bool? IsDimmable { get; set; }
}
