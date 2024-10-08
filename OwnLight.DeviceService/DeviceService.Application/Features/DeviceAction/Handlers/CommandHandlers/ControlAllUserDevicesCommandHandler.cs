using DeviceService.Application.Features.DeviceAction.Commands;
using DeviceService.Domain.Enums;
using DeviceService.Domain.Interfaces;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Entity = DeviceService.Domain.Entities;

namespace DeviceService.Application.Features.DeviceAction.Handlers.CommandHandlers;

public class ControlAllUserDevicesCommandHandler(
    IDeviceRepository deviceRepository,
    IDeviceActionRepository deviceActionRepository,
    IHttpContextAccessor httpContextAccessor,
    IValidator<ControlAllUserDevicesCommand> validator
) : IRequestHandler<ControlAllUserDevicesCommand>
{
    private readonly IDeviceRepository _deviceRepository = deviceRepository;
    private readonly IDeviceActionRepository _deviceActionRepository = deviceActionRepository;
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
    private readonly IValidator<ControlAllUserDevicesCommand> _validator = validator;

    public async Task<Unit> Handle(
        ControlAllUserDevicesCommand request,
        CancellationToken cancellationToken
    )
    {
        await _validator.ValidateAndThrowAsync(request, cancellationToken: cancellationToken);

        var userId = _httpContextAccessor.HttpContext?.Items["UserId"]?.ToString();

        if (string.IsNullOrEmpty(userId))
            throw new UnauthorizedAccessException("Usuário não autenticado.");

        var devices = await _deviceRepository.GetDevicesByUserIdAsync(
            Guid.Parse(userId),
            pageNumber: 1,
            pageSize: 30
        );

        if (!devices.Any())
            throw new Exception("Nenhum dispositivo encontrado.");

        var actionsToLog = new List<Entity.DeviceAction>();

        foreach (var device in devices)
        {
            if (device.UserId.ToString() != userId)
            {
                throw new UnauthorizedAccessException(
                    $"O dispositivo de id {device.Id} não pertence ao usuário."
                );
            }

            device.Status = request.Status;

            if (device.IsDimmable ?? false)
                device.Brightness = request.Status == DeviceStatus.On ? 100 : 0;

            var deviceAction = new Entity.DeviceAction
            {
                DeviceId = device.Id,
                UserId = Guid.Parse(userId),
                Action =
                    request.Status == DeviceStatus.On
                        ? DeviceActions.TurnOn
                        : DeviceActions.TurnOff,
                Status = ActionStatus.Success,
            };

            actionsToLog.Add(deviceAction);
        }

        await _deviceRepository.ControlAllUserDevicesAsync(Guid.Parse(userId), request.Status);

        await _deviceActionRepository.AddDeviceActionsAsync(actionsToLog);

        return Unit.Value;
    }
}
