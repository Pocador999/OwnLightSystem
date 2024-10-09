using AutomationService.Domain.Entities;

namespace AutomationService.Domain.Interfaces;

public interface IRoomRepository : IRepository<Room>
{
    Task<IEnumerable<Room>> GetUserRoomsAsync(
        Guid userId,
        int page,
        int pageSize,
        CancellationToken cancellationToken = default
    );
    Task<Room?> GetRoomByNameAsync(
        string roomName,
        CancellationToken cancellationToken = default
    );
}
