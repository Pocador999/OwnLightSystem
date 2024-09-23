using DeviceService.Domain.Entities;

namespace DeviceService.Domain.Interfaces;

public interface IDeviceActionRepository : IRepository<DeviceAction>
{
    Task<IEnumerable<DeviceAction>> GetUserDeviceActionsByDeviceAsync(Guid userId, Guid deviceId);
    Task<IEnumerable<DeviceAction>> GetUserDeviceActionsByRoomAsync(Guid userId, Guid roomId);
    Task<IEnumerable<DeviceAction>> GetUserDeviceActionsByGroupAsync(Guid userId, Guid groupId);
    Task<IEnumerable<DeviceAction>> GetUserDeviceActionsByTypeAsync(Guid userId, Guid deviceType);
}
