using System.Text.Json.Serialization;
using MediatR;

namespace AutomationService.Application.Features.Group.Commands;

public class AddDevicesCommand(Guid groupId, Guid[] deviceIds) : IRequest
{
    [JsonIgnore]
    public Guid GroupId { get; set; } = groupId;
    public Guid[] DeviceIds { get; set; } = deviceIds;
}
