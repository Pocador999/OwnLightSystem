using AutomationService.Domain.Entities;
using AutomationService.Domain.Enums;
using AutomationService.Domain.Interfaces;
using AutomationService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AutomationService.Infrastructure.Repositories;

public class RoutineRepository(DataContext dataContext)
    : Repository<Routine>(dataContext),
        IRoutineRepository
{
    private readonly DbSet<Routine> _routines = dataContext.Set<Routine>();
    private readonly DbSet<RoutineExecutionLog> _routineExecutionLogs =
        dataContext.Set<RoutineExecutionLog>();

    public async Task<IEnumerable<Routine>> GetUserRoutinesAsync(
        Guid userId,
        int page,
        int pageSize,
        CancellationToken cancellationToken = default
    )
    {
        var skipAmount = (page - 1) * pageSize;
        return await _routines
            .Where(r => r.UserId == userId)
            .Skip(skipAmount)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
    }

    public async Task<Routine?> GetRoutineByNameAsync(
        string name,
        CancellationToken cancellationToken = default
    ) => await _routines.FirstOrDefaultAsync(r => r.Name == name, cancellationToken);

    public async Task<IEnumerable<Routine>> GetRoutinesToExecuteAsync(
        TimeSpan currentTime,
        CancellationToken cancellationToken = default
    )
    {
        var routinesToExecute = await _routines
            .Where(r => r.ExecutionTime <= currentTime)
            .Where(r =>
                !_routineExecutionLogs.Any(log =>
                    log.RoutineId == r.Id && log.ExecutedAt.Date == DateTime.UtcNow.Date
                )
            )
            .ToListAsync(cancellationToken);

        foreach (var routine in routinesToExecute)
        {
            if (DateTime.UtcNow.TimeOfDay > routine.ExecutionTime.Add(TimeSpan.FromMinutes(30)))
                await MarkRoutineAsFailedAsync(routine, cancellationToken);
        }

        return routinesToExecute;
    }

    private async Task MarkRoutineAsFailedAsync(
        Routine routine,
        CancellationToken cancellationToken
    )
    {
        var log = new RoutineExecutionLog
        {
            Id = Guid.NewGuid(),
            RoutineId = routine.Id,
            ActionTarget = routine.ActionTarget,
            TargetId = routine.TargetId,
            ActionType = routine.ActionType,
            ActionStatus = ActionStatus.Failed,
            ErrorMessage = "Rotina não foi executada no tempo estipulado.",
        };

        await _routineExecutionLogs.AddAsync(log, cancellationToken);
        await _dataContext.SaveChangesAsync(cancellationToken);
    }
}
