using AutomationService.Application.Common.Jobs;
using AutomationService.Application.Common.Services.Interfaces;
using AutomationService.Domain.Entities;
using Quartz;

namespace AutomationService.Application.Common.Services;

public class RoutineSchedulerService(ISchedulerFactory schedulerFactory) : IRoutineSchedulerService
{
    private readonly ISchedulerFactory _schedulerFactory = schedulerFactory;

    public async Task ScheduleRoutineAsync(Routine routine)
    {
        var scheduler = await _schedulerFactory.GetScheduler();

        var job = JobBuilder
            .Create<RoutineJob>()
            .WithIdentity($"RoutineJob-{routine.Id}")
            .UsingJobData("RoutineId", routine.Id.ToString())
            .Build();

        var trigger = TriggerBuilder
            .Create()
            .WithIdentity($"RoutineTrigger-{routine.Id}")
            .StartNow()
            .WithSchedule(
                CronScheduleBuilder.DailyAtHourAndMinute(
                    routine.ExecutionTime.Hours,
                    routine.ExecutionTime.Minutes
                )
            )
            .Build();

        await scheduler.ScheduleJob(job, trigger);
    }

    public async Task DeleteRoutineAsync(Guid routineId)
    {
        var scheduler = await _schedulerFactory.GetScheduler();

        var jobKey = new JobKey($"RoutineJob-{routineId}");
        await scheduler.DeleteJob(jobKey);
    }

    public async Task UpdateRoutineAsync(Routine routine)
    {
        var scheduler = await _schedulerFactory.GetScheduler();

        var jobKey = new JobKey($"RoutineJob-{routine.Id}");
        await scheduler.DeleteJob(jobKey);

        var job = JobBuilder
            .Create<RoutineJob>()
            .WithIdentity($"RoutineJob-{routine.Id}")
            .UsingJobData("RoutineId", routine.Id.ToString())
            .Build();

        var trigger = TriggerBuilder
            .Create()
            .WithIdentity($"RoutineTrigger-{routine.Id}")
            .StartNow()
            .WithSchedule(
                CronScheduleBuilder.DailyAtHourAndMinute(
                    routine.ExecutionTime.Hours,
                    routine.ExecutionTime.Minutes
                )
            )
            .Build();

        await scheduler.ScheduleJob(job, trigger);
    }
}
