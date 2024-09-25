using DeviceService.Domain.Enums;

namespace DeviceService.Application.DTOs;

public class DeviceResponseDTO
{
    public Guid Id { get; set; }
    public required string DeviceType { get; set; }
    public Guid UserId { get; set; }
    public Guid RoomId { get; set; }
    public Guid? GroupId { get; set; }
    public required string Name { get; set; }
    public bool? IsDimmable { get; set; }
    public int? Brightness { get; set; }
    public DeviceStatus Status { get; set; }
}
