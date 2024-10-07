using AutomationService.Domain.Entities;
using AutomationService.Domain.Interfaces;
using AutomationService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AutomationService.Infrastructure.Repositories;

public class RoomRepository(DataContext dataContext)
    : Repository<Room>(dataContext),
        IRoomRepository
{
    private readonly DbSet<Room> _dbSet = dataContext.Set<Room>();

    public async Task<IEnumerable<Room>> GetUserRoomsAsync(Guid userId, int page, int pageSize)
    {
        var skipAmount = (page - 1) * pageSize;
        return await _dbSet
            .Where(r => r.UserId == userId)
            .Skip(skipAmount)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<Room?> GetRoomByNameAsync(string name) =>
        await _dbSet.FirstOrDefaultAsync(r => r.Name == name);
}
