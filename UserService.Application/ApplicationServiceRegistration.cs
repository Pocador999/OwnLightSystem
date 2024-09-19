using System.Reflection;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using UserService.Application.Common.Services.Messages;
using UserService.Application.Common.Validation;
using UserService.Application.Features.User.Commands;

namespace UserService.Application;

public static class ApplicationServiceRegistration
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        services.AddMediatR(Assembly.GetExecutingAssembly());

        services.AddTransient<IValidator<CreateCommand>, CreateCommandValidator>();
        services.AddTransient<IValidator<UpdateCommand>, UpdateCommandValidator>();
        services.AddTransient<IValidator<UpdatePasswordCommand>, UpdatePasswordCommandValidator>();

        services.AddScoped<IMessageService, MessageService>();

        return services;
    }
}
