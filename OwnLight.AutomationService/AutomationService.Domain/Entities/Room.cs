namespace AutomationService.Domain.Entities;

public class Room
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Name { get; set; } = null!;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; }

    public ICollection<Guid> DeviceIds { get; set; } = [];
}
