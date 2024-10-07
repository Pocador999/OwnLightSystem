using DeviceService.Application.Features.DeviceAction.Commands;
using DeviceService.Domain.Enums;
using DeviceService.Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;
using Entity = DeviceService.Domain.Entities;

namespace DeviceService.Application.Features.DeviceAction.Handlers;

public class DimmerizeDeviceCommandHandler(
    IDeviceRepository deviceRepository,
    IDeviceActionRepository deviceActionRepository,
    IHttpContextAccessor httpContextAccessor
) : IRequestHandler<DimmerizeDeviceCommand>
{
    private readonly IDeviceRepository _deviceRepository = deviceRepository;
    private readonly IDeviceActionRepository _deviceActionRepository = deviceActionRepository;
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    public async Task<Unit> Handle(
        DimmerizeDeviceCommand request,
        CancellationToken cancellationToken
    )
    {
        var userId = _httpContextAccessor.HttpContext?.Items["UserId"]?.ToString();

        if (string.IsNullOrEmpty(userId))
            throw new UnauthorizedAccessException("Usuário não autenticado.");

        var device =
            await _deviceRepository.GetByIdAsync(request.DeviceId)
            ?? throw new KeyNotFoundException(
                $"Dispositivo de id {request.DeviceId} não encontrado."
            );

        // Verifica se o dispositivo pertence ao usuário que fez a requisição
        if (device.UserId.ToString() != userId)
            throw new UnauthorizedAccessException(
                $"O dispositivo de id {request.DeviceId} não pertence ao usuário."
            );

        if (device.IsDimmable == false)
            throw new InvalidOperationException("Este dispositivo não suporta ajuste de brilho.");

        // Valida o valor de brilho
        if (request.Brightness < 0 || request.Brightness > 100)
            throw new ArgumentOutOfRangeException("Brightness deve estar entre 0 e 100.");

        try
        {
            // Se o brilho for maior que 0 e o dispositivo estiver desligado, liga o dispositivo
            if (request.Brightness > 0 && device.Status == DeviceStatus.Off)
            {
                device = await _deviceRepository.ControlDeviceAsync(
                    request.DeviceId,
                    DeviceStatus.On
                );
            }
            // Se o brilho for 0 e o dispositivo estiver ligado, desliga o dispositivo
            else if (request.Brightness == 0 && device.Status == DeviceStatus.On)
            {
                device = await _deviceRepository.ControlDeviceAsync(
                    request.DeviceId,
                    DeviceStatus.Off
                );
            }

            // Ajusta o brilho
            device.Brightness = request.Brightness;
            await _deviceRepository.UpdateAsync(device);

            // Cria um registro de ação no banco de dados
            var deviceAction = new Entity.DeviceAction
            {
                DeviceId = device.Id,
                UserId = Guid.Parse(userId),
                Action = DeviceActions.Dim,
                Status = ActionStatus.Success,
            };

            await _deviceActionRepository.CreateAsync(deviceAction);
        }
        catch (Exception)
        {
            // Caso ocorra algum erro, cria um registro com status "Failed"
            var deviceAction = new Entity.DeviceAction
            {
                DeviceId = device.Id,
                UserId = Guid.Parse(userId),
                Action = DeviceActions.Dim,
                Status = ActionStatus.Failed,
            };

            await _deviceActionRepository.CreateAsync(deviceAction);
            throw;
        }

        return Unit.Value;
    }
}
