using System.Reflection;
using AutomationService.Application.Common.Services;
using AutomationService.Application.Common.Services.Interfaces;
using AutomationService.Application.Common.Validations.Routine;
using AutomationService.Application.Features.Routine.Commands;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Quartz;

namespace AutomationService.Application;

public static class ApplicationServiceRegistration
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IRoutineSchedulerService, RoutineSchedulerService>();

        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        services.AddMediatR(Assembly.GetExecutingAssembly());

        services.AddHttpClient<IDeviceServiceClient, DeviceServiceClient>(client =>
            client.BaseAddress = new Uri("http://localhost:5034") // URL do DeviceService
        );
        services.AddHttpClient<IUserServiceClient, UserServiceClient>(client =>
            client.BaseAddress = new Uri("http://localhost:5008") // URL do UserService
        );

        services.AddQuartz(q =>
        {
            q.SchedulerId = "AutomationService";
            q.UseSimpleTypeLoader();
            q.UseInMemoryStore();
        });

        services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);

        services.AddTransient<IValidator<CreateRoutineCommand>, CreateRoutineValidator>();
        services.AddTransient<IValidator<UpdateRoutineCommand>, UpdateRoutineValidator>();
        services.AddTransient<IValidator<UpdateRoutineNameCommand>, UpdateRoutineNameValidator>();

        return services;
    }
}
