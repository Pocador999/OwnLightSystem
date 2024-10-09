using AutomationService.Domain.Entities;

namespace AutomationService.Domain.Interfaces;

public interface IRoutineRepository : IRepository<Routine>
{
    Task<IEnumerable<Routine>> GetUserRoutinesAsync(
        Guid userId,
        int page,
        int pageSize,
        CancellationToken cancellationToken = default
    );
    Task<Routine?> GetRoutineByNameAsync(
        string routineName,
        CancellationToken cancellationToken = default
    );

    Task<IEnumerable<Routine>> GetRoutinesToExecuteAsync(
        TimeSpan currentTime,
        CancellationToken cancellationToken = default
    );
}
