using AutomationService.Domain.Entities;

namespace AutomationService.Domain.Interfaces;

public interface IGroupRepository : IRepository<Group>
{
    public Task<IEnumerable<Group>> GetUserGroupsAsync(Guid userId, int page, int pageSize);
    public Task<Group?> GetGroupByNameAsync(string name);
}
