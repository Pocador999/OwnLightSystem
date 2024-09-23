using DeviceService.Domain.Entities;

namespace DeviceService.Domain.Interfaces;

public interface IDeviceRepository : IRepository<Device>
{
    Task<IEnumerable<Device>> GetUserDevicesAsync(Guid userId, int page, int pageSize);
    Task<IEnumerable<Device>> GetUserDevicesByRoomAsync(Guid userId, Guid roomId);
    Task<IEnumerable<Device>> GetUserDevicesByGroupAsync(Guid userId, Guid groupId);
    Task<IEnumerable<Device>> GetUserDevicesByTypeAsync(Guid userId, Guid deviceType);
    Task<IEnumerable<Device>> DeleteUserDevicesAsync(Guid userId);
    Task<IEnumerable<Device>> DeleteUserDevicesByRoomAsync(Guid userId, Guid roomId);
    Task<IEnumerable<Device>> DeleteUserDevicesByGroupAsync(Guid userId, Guid groupId);
    Task<IEnumerable<Device>> DeleteUserDevicesByTypeAsync(Guid userId, Guid deviceType);
}
