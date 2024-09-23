using System.ComponentModel.DataAnnotations;

namespace DeviceService.Domain.Primitives;

public class Entity
{
    [Key]
    public Guid Id { get; protected set; } = Guid.NewGuid();

    [Timestamp, Required]
    public DateTime CreatedAt { get; protected set; } = DateTime.UtcNow;

    [Timestamp]
    public DateTime? UpdatedAt { get; protected set; } = DateTime.UtcNow;
}
