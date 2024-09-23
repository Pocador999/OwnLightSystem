using DeviceService.Domain.Primitives;

namespace DeviceService.Domain.Entities;

public class DeviceType : Entity
{
    public required string TypeName { get; set; }

    public virtual required ICollection<Device> Devices { get; set; }
}
