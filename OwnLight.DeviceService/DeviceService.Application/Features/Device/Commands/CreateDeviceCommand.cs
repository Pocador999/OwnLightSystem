using System.Text.Json.Serialization;
using MediatR;

namespace DeviceService.Application.Features.Device.Commands;

public class CreateDeviceCommand(string deviceType, string name) : IRequest<Guid>
{
    [JsonIgnore]
    public Guid UserId { get; set; }

    public Guid RoomId { get; set; } = Guid.Empty;

    [JsonIgnore]
    public Guid? GroupId { get; set; } = Guid.Empty;

    [JsonIgnore]
    public int? Brightness { get; set; }

    public required string Name { get; set; } = name;
    public bool IsDimmable { get; set; }
    public required string DeviceType { get; set; } = deviceType;
}
