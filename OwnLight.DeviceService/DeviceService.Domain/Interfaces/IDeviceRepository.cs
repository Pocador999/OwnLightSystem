using DeviceService.Domain.Entities;
using DeviceService.Domain.Enums;

namespace DeviceService.Domain.Interfaces;

public interface IDeviceRepository : IRepository<Device>
{
    Task<Device> ControlDeviceAsync(Guid deviceId, DeviceStatus status);
    Task<Device> SwitchAsync(Guid deviceId, DeviceStatus status);
    Task<Device> ControlBrightnessAsync(Guid deviceId, int brightness);
    Task<int> ControlUserDevicesByRoomIdAsync(Guid userId, Guid roomId, DeviceStatus status);
    Task<int> ControlUserDevicesByGroupIdAsync(Guid userId, Guid groupId, DeviceStatus status);
    Task<int> ControlBrightnessByUserRoomAsync(Guid userId, Guid roomId, int brightness);
    Task<int> ControlBrightnessByUserGroupAsync(Guid userId, Guid groupId, int brightness);
    Task<int> ControlAllUserDevicesAsync(Guid userId, DeviceStatus status);

    Task<IEnumerable<Device>> GetDevicesByIdsAsync(Guid[] deviceIds, int pageNumber, int pageSize);
    Task<IEnumerable<Device>> GetDevicesByUserIdAsync(Guid userId, int pageNumber, int pageSize);

    Task<IEnumerable<Device>> GetUserDevicesByRoomIdAsync(
        Guid userId,
        Guid roomId,
        int pageNumber,
        int pageSize
    );
    Task<IEnumerable<Device>> GetUserDevicesByGroupIdAsync(
        Guid userId,
        Guid groupId,
        int pageNumber,
        int pageSize
    );
}
