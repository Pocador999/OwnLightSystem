using AutomationService.Domain.Entities;

namespace AutomationService.Domain.Interfaces;

public interface IRoutineRepository : IRepository<Routine>
{
    public Task<IEnumerable<Routine>> GetUserRoutinesAsync(Guid userId);
    public Task<Routine?> GetRoutineByNameAsync(string name);
}
