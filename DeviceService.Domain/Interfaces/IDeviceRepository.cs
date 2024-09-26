using DeviceService.Domain.Entities;
using DeviceService.Domain.Enums;

namespace DeviceService.Domain.Interfaces;

public interface IDeviceRepository : IRepository<Device>
{
    // Task<IEnumerable<Device>> GetUserDevicesByRoomAsync(Guid userId, Guid roomId);
    // Task<IEnumerable<Device>> GetUserDevicesByGroupAsync(Guid userId, Guid groupId);
    // Task<IEnumerable<Device>> GetUserDevicesByTypeAsync(Guid userId, Guid deviceType);
    // Task<IEnumerable<Device>> DeleteUserDevicesAsync(Guid userId);
    // Task<IEnumerable<Device>> DeleteUserDevicesByRoomAsync(Guid userId, Guid roomId);
    // Task<IEnumerable<Device>> DeleteUserDevicesByTypeAsync(Guid userId, Guid deviceType);
    Task<Device> ControlDeviceAsync(Guid deviceId, DeviceStatus status);
    Task<Device> SwitchAsync(Guid deviceId, DeviceStatus status);
    Task<Device> ControlBrightnessAsync(Guid deviceId, int brightness);
}
