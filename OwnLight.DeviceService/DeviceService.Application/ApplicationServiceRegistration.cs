using System.Reflection;
using DeviceService.Application.Common.Validations.Device;
using DeviceService.Application.Common.Validations.DeviceAction;
using DeviceService.Application.Features.Device.Commands;
using DeviceService.Application.Features.DeviceAction.Commands;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace DeviceService.Application;

public static class ApplicationServiceRegistration
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        services.AddMediatR(Assembly.GetExecutingAssembly());

        services.AddTransient<IValidator<CreateDeviceCommand>, CreateDeviceValidator>();
        services.AddTransient<IValidator<UpdateDeviceCommand>, UpdateDeviceValidator>();
        services.AddTransient<IValidator<UpdateDeviceRoomCommand>, UpdateDeviceRoomValidator>();
        services.AddTransient<IValidator<UpdateDeviceGroupCommand>, UpdateDeviceGroupValidator>();

        services.AddTransient<IValidator<ControlAllUserDevicesCommand>, ControlAllUserDevicesValidator>();
        services.AddTransient<IValidator<ControlDeviceCommand>, ControlDeviceValidator>();
        services.AddTransient<IValidator<ControlGroupCommand>, ControlGroupValidator>();
        services.AddTransient<IValidator<ControlRoomCommand>, ControlRoomValidator>();
        services.AddTransient<IValidator<DimmerizeDeviceCommand>, DimmerizeDeviceValidator>();
        services.AddTransient<IValidator<DimmerizeGroupCommand>, DimmerizeGroupValidator>();
        services.AddTransient<IValidator<DimmerizeRoomCommand>, DimmerizeRoomValidator>();

        return services;
    }
}
