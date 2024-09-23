using System.ComponentModel.DataAnnotations;

namespace DeviceService.Domain.Primitives;

public class Entity
{
    [Key]
    public Guid Id { get; protected set; } = Guid.NewGuid();
    public DateTime CreatedAt { get; protected set; } = DateTime.UtcNow;
}
