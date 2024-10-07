using AutomationService.Domain.Entities;

namespace AutomationService.Domain.Interfaces;

public interface IRoomRepository : IRepository<Room>
{
    public Task<IEnumerable<Room>> GetUserRoomsAsync(Guid userId);
    public Task<Room?> GetRoomByNameAsync(string name);
}
