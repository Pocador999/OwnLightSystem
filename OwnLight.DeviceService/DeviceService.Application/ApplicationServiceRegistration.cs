using System.Reflection;
using DeviceService.Application.Common.Validations.Device;
using DeviceService.Application.Features.Device.Commands;
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

        return services;
    }
}
