using DeviceService.Application.Features.DeviceAction.Commands;
using DeviceService.Domain.Enums;
using DeviceService.Domain.Interfaces;
using MediatR;
using Entity = DeviceService.Domain.Entities;

namespace DeviceService.Application.Features.DeviceAction.Handlers;

public class ControlDeviceCommandHandler(
    IDeviceRepository deviceRepository,
    IDeviceActionRepository deviceActionRepository
) : IRequestHandler<ControlDeviceCommand>
{
    private readonly IDeviceRepository _deviceRepository = deviceRepository;
    private readonly IDeviceActionRepository _deviceActionRepository = deviceActionRepository;

    public async Task<Unit> Handle(
        ControlDeviceCommand request,
        CancellationToken cancellationToken
    )
    {

        var device =
            await _deviceRepository.GetByIdAsync(request.DeviceId)
            ?? throw new KeyNotFoundException($"Device with id {request.DeviceId} not found.");
        try
        {
            // Controla o dispositivo (liga/desliga)
            device = await _deviceRepository.ControlDeviceAsync(request.DeviceId, request.Status);

            // Cria um registro de ação no banco de dados
            var deviceAction = new Entity.DeviceAction
            {
                DeviceId = device.Id,
                Action =
                    request.Status == DeviceStatus.On
                        ? DeviceActions.TurnOn
                        : DeviceActions.TurnOff,
                Status = ActionStatus.Success,
            };

            await _deviceActionRepository.AddDeviceActionAsync(deviceAction);
        }
        catch (Exception)
        {
            // Caso ocorra algum erro, cria um registro com status "Failed"
            var deviceAction = new Entity.DeviceAction
            {
                DeviceId = device.Id,
                Action =
                    request.Status == DeviceStatus.On
                        ? DeviceActions.TurnOn
                        : DeviceActions.TurnOff,
                Status = ActionStatus.Failed,
            };

            await _deviceActionRepository.AddDeviceActionAsync(deviceAction);
            throw;
        }

        return Unit.Value;
    }
}
 