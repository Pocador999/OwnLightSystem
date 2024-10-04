using DeviceService.Domain.Entities;

namespace DeviceService.Domain.Interfaces;

public interface IDeviceTypeRepository : IRepository<DeviceType>
{
    Task<DeviceType?> GetDeviceTypeByNameAsync(string typeName);
    Task<IEnumerable<DeviceType>> GetUserDeviceTypesAsync(Guid userId);
}
