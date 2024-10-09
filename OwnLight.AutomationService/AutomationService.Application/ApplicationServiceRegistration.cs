using System.Reflection;
using AutomationService.Application.Common.Jobs;
using AutomationService.Application.Common.Services;
using AutomationService.Application.Common.Services.Interfaces;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Quartz;

namespace AutomationService.Application;

public static class ApplicationServiceRegistration
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        services.AddMediatR(Assembly.GetExecutingAssembly());

        services.AddScoped<IActionService, ActionService>();
        services.AddScoped<IRoutineExecutionLogService, RoutineExecutionLogService>();

        services.AddQuartz(q =>
        {
            var jobKey = new JobKey("RoutineJob");

            q.AddJob<RoutineJob>(opts => opts.WithIdentity(jobKey));

            q.AddTrigger(opts =>
                opts.ForJob(jobKey)
                    .WithIdentity("RoutineJob-trigger")
                    .WithDailyTimeIntervalSchedule(
                        24,
                        IntervalUnit.Hour,
                        x => x.OnEveryDay().StartingDailyAt(TimeOfDay.HourAndMinuteOfDay(19, 15))
                    )
            );
        });

        services.AddHttpContextAccessor();
        services.AddHttpClient<IActionService, ActionService>();
        services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);

        return services;
    }
}
