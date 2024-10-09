using AutomationService.Domain.Entities;

namespace AutomationService.Application.Common.Services.Interfaces;

public interface IRoutineService
{
    public Task<Routine> GetRoutineByIdAsync(
        Guid routineId,
        CancellationToken cancellationToken = default
    );

    public Task<Routine?> GetRoutineByNameAsync(
        string routineName,
        CancellationToken cancellationToken = default
    );

    public Task<IEnumerable<Routine>> GetUserRoutinesAsync(
        Guid userId,
        int page,
        int pageSize,
        CancellationToken cancellationToken = default
    );

    public Task CreateRoutineAsync(Routine routine, CancellationToken cancellationToken = default);
    public Task UpdateRoutineAsync(Routine routine, CancellationToken cancellationToken = default);
    public Task DeleteRoutineAsync(Guid routineId, CancellationToken cancellationToken = default);
}
