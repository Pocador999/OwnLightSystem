using DeviceService.Domain.Entities;

namespace DeviceService.Domain.Interfaces;

public interface IDeviceTypeRepository : IRepository<DeviceType>
{
    Task<DeviceType?> GetDeviceTypeByNameAsync(string typeName);
    Task<IEnumerable<DeviceType>> GetUserDeviceTypesAsync(Guid userId);
    Task<IEnumerable<DeviceType>> GetUserDeviceTypesByRoomAsync(Guid userId, Guid roomId);
    Task<IEnumerable<DeviceType>> GetUserDeviceTypesByGroupAsync(Guid userId, Guid groupId);
}
