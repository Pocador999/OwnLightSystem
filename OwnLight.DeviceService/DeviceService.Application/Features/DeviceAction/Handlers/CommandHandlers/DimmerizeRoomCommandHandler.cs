using DeviceService.Application.Features.DeviceAction.Commands;
using DeviceService.Domain.Enums;
using DeviceService.Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;
using Entity = DeviceService.Domain.Entities;

namespace DeviceService.Application.Features.DeviceAction.Handlers.CommandHandlers;

public class DimmerizeRoomCommandHandler(
    IDeviceRepository deviceRepository,
    IDeviceActionRepository deviceActionRepository,
    IHttpContextAccessor httpContextAccessor
) : IRequestHandler<DimmerizeRoomCommand>
{
    private readonly IDeviceRepository _deviceRepository = deviceRepository;
    private readonly IDeviceActionRepository _deviceActionRepository = deviceActionRepository;
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    public async Task<Unit> Handle(
        DimmerizeRoomCommand request,
        CancellationToken cancellationToken
    )
    {
        var userId = _httpContextAccessor.HttpContext?.Items["UserId"]?.ToString();

        if (string.IsNullOrEmpty(userId))
            throw new UnauthorizedAccessException("Usuário não autenticado.");

        var devices =
            await _deviceRepository.GetUserDevicesByRoomIdAsync(
                Guid.Parse(userId),
                request.RoomId,
                pageNumber: 1,
                pageSize: 30
            ) ?? throw new Exception("Dispositivos não encontrados.");

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

                if (device.IsDimmable == true)
                {
                    if (request.Brightness == 0)
                    {
                        device.Status = DeviceStatus.Off;
                        device.Brightness = 0;
                    }
                    else
                    {
                        device.Brightness = request.Brightness;
                        device.Status = DeviceStatus.On;
                    }

                    var deviceAction = new Entity.DeviceAction
                    {
                        DeviceId = device.Id,
                        UserId = Guid.Parse(userId),
                        Action =
                            request.Brightness == 0 ? DeviceActions.TurnOff : DeviceActions.Dim,
                        Status = ActionStatus.Success,
                    };

                    actionsToLog.Add(deviceAction);
                }
            }

            await _deviceRepository.ControlBrightnessByUserRoomAsync(
                Guid.Parse(userId),
                request.RoomId,
                request.Brightness,
                request.Brightness == 0 ? DeviceStatus.Off : DeviceStatus.On
            );

            await _deviceActionRepository.AddDeviceActionsAsync(actionsToLog);
        }
        catch (Exception ex)
        {
            throw new Exception("Erro ao dimmerizar sala.", ex);
        }

        return Unit.Value;
    }
}
