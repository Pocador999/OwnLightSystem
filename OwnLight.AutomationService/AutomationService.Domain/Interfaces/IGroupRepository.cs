using AutomationService.Domain.Entities;

namespace AutomationService.Domain.Interfaces;

public interface IGroupRepository : IRepository<Group>
{
    Task<IEnumerable<Group>> GetUserGroupsAsync(
        Guid userId,
        int page,
        int pageSize,
        CancellationToken cancellationToken = default
    );
    Task<Group?> GetGroupByNameAsync(
        string groupName,
        CancellationToken cancellationToken = default
    );
}
