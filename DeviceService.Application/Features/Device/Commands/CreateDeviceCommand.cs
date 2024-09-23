using System.Text.Json.Serialization;
using MediatR;

namespace DeviceService.Application.Features.Device.Commands;

public class CreateDeviceCommand(string deviceType, string name) : IRequest<Guid>
{
    public required string DeviceType { get; set; } = deviceType;

    [JsonIgnore]
    public Guid UserId { get; set; } = Guid.NewGuid();

    [JsonIgnore]
    public Guid RoomId { get; set; } = Guid.NewGuid();

    [JsonIgnore]
    public Guid? GroupId { get; set; } = Guid.Empty;
    public required string Name { get; set; } = name;
    public bool? IsDimmable { get; set; }
    public int? Brightness { get; set; }
}
