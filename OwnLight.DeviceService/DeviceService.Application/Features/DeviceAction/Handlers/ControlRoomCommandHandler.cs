using DeviceService.Application.Features.DeviceAction.Commands;
using DeviceService.Domain.Enums;
using DeviceService.Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;
using Entity = DeviceService.Domain.Entities;

namespace DeviceService.Application.Features.DeviceAction.Handlers;

public class ControlRoomCommandHandler(
    IDeviceRepository deviceRepository,
    IDeviceActionRepository deviceActionRepository,
    IHttpContextAccessor httpContextAccessor
) : IRequestHandler<ControlRoomCommand>
{
    private readonly IDeviceRepository _deviceRepository = deviceRepository;
    private readonly IDeviceActionRepository _deviceActionRepository = deviceActionRepository;
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    public async Task<Unit> Handle(ControlRoomCommand request, CancellationToken cancellationToken)
    {
        var userId = _httpContextAccessor.HttpContext?.Items["UserId"]?.ToString();

        if (string.IsNullOrEmpty(userId))
            throw new UnauthorizedAccessException("Usuário não autenticado.");

        var devices = await _deviceRepository.GetUserDevicesByRoomIdAsync(
            Guid.Parse(userId),
            request.RoomId,
            pageNumber: 1,
            pageSize: 30
        );

        var actionsToLog = new List<Entity.DeviceAction>();

        try
        {
            foreach (var device in devices)
            {
                if (device.UserId.ToString() != userId)
                {
                    throw new UnauthorizedAccessException(
                        $"O dispositivo de id {device.Id} não pertence ao usuário."
                    );
                }
                device.Status = request.Status;

                if (device.IsDimmable == true)
                    device.Brightness = request.Status == DeviceStatus.On ? 100 : 0;
                else
                    device.Brightness = null;

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

            await _deviceRepository.ControlUserDevicesByRoomIdAsync(
                Guid.Parse(userId),
                request.RoomId,
                request.Status
            );

            await _deviceActionRepository.AddDeviceActionsAsync(actionsToLog);
        }
        catch (Exception)
        {
            foreach (var device in devices)
            {
                var deviceActionFailed = new Entity.DeviceAction
                {
                    DeviceId = device.Id,
                    UserId = Guid.Parse(userId),
                    Action =
                        request.Status == DeviceStatus.On
                            ? DeviceActions.TurnOn
                            : DeviceActions.TurnOff,
                    Status = ActionStatus.Failed,
                };

                await _deviceActionRepository.CreateAsync(deviceActionFailed);
            }

            throw;
        }

        return Unit.Value;
    }
}
