using DeviceService.Domain.Entities;
using DeviceService.Domain.Enums;
using DeviceService.Domain.Interfaces;
using DeviceService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DeviceService.Infrastructure.Repositories;

public class DeviceRepository(DataContext dataContext)
    : Repository<Device>(dataContext),
        IDeviceRepository
{
    private readonly DbSet<Device> _dbSet = dataContext.Set<Device>();

    public async Task<Device> ControlDeviceAsync(Guid deviceId, DeviceStatus status)
    {
        var device =
            await _dbSet.FindAsync(deviceId) ?? throw new KeyNotFoundException("Device not found");

        device.Status = status;
        await SaveChangesAsync();
        return device;
    }

    public async Task<Device> SwitchAsync(Guid deviceId, DeviceStatus status)
    {
        var device =
            await _dbSet.FindAsync(deviceId) ?? throw new KeyNotFoundException("Device not found");

        device.Status = device.Status == DeviceStatus.On ? DeviceStatus.Off : DeviceStatus.On;
        await SaveChangesAsync();
        return device;
    }

    public async Task<Device> ControlBrightnessAsync(Guid deviceId, int brightness)
    {
        var device =
            await _dbSet.FindAsync(deviceId) ?? throw new KeyNotFoundException("Device not found");
        device.Brightness = brightness;
        await SaveChangesAsync();
        return device;
    }

    public async Task<IEnumerable<Device>> GetDevicesByIdsAsync(
        Guid[] deviceIds,
        int pageNumber,
        int pageSize
    )
    {
        var skipAmount = (pageNumber - 1) * pageSize;
        return await _dbSet
            .Where(d => deviceIds.Contains(d.Id))
            .Skip(skipAmount)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<IEnumerable<Device>> GetDevicesByUserIdAsync(
        Guid userId,
        int pageNumber,
        int pageSize
    )
    {
        var skipAmount = (pageNumber - 1) * pageSize;
        return await _dbSet
            .Include(d => d.DeviceType)
            .Where(d => d.UserId == userId)
            .Skip(skipAmount)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<IEnumerable<Device>> GetUserDevicesByRoomIdAsync(
        Guid userId,
        Guid roomId,
        int pageNumber,
        int pageSize
    )
    {
        var skipAmount = (pageNumber - 1) * pageSize;
        return await _dbSet
            .Include(d => d.DeviceType)
            .Where(d => d.UserId == userId && d.RoomId == roomId)
            .Skip(skipAmount)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<IEnumerable<Device>> GetUserDevicesByGroupIdAsync(
        Guid userId,
        Guid groupId,
        int pageNumber,
        int pageSize
    )
    {
        var skipAmount = (pageNumber - 1) * pageSize;
        return await _dbSet
            .Include(d => d.DeviceType)
            .Where(d => d.UserId == userId && d.GroupId == groupId)
            .Skip(skipAmount)
            .Take(pageSize)
            .ToListAsync();
    }
}
