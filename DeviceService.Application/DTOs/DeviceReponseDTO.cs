namespace DeviceService.Application.DTOs;

public class DeviceReponseDTO
{
    public Guid Id { get; set; }
    public required string DeviceType { get; set; }
    public Guid UserId { get; set; }
    public Guid RoomId { get; set; }
    public Guid? GroupId { get; set; }
    public required string Name { get; set; }
    public bool? IsDimmable { get; set; }
}
