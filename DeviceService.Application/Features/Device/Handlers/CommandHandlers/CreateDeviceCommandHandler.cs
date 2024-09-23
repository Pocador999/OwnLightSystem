using AutoMapper;
using DeviceService.Application.Features.Device.Commands;
using DeviceService.Domain.Interfaces;
using MediatR;
using Entity = DeviceService.Domain.Entities;

namespace DeviceService.Application.Features.Device.Handlers.CommandHandlers;

public class CreateDeviceCommandHandler(
    IDeviceRepository deviceRepository,
    IDeviceTypeRepository deviceTypeRepository,
    IMapper mapper
) : IRequestHandler<CreateDeviceCommand, Guid>
{
    private readonly IDeviceRepository _deviceRepository = deviceRepository;
    private readonly IDeviceTypeRepository _deviceTypeRepository = deviceTypeRepository;
    private readonly IMapper _mapper = mapper;

    public async Task<Guid> Handle(CreateDeviceCommand request, CancellationToken cancellationToken)
    {
        var deviceType =
            await _deviceTypeRepository.GetDeviceTypeByNameAsync(request.DeviceType)
            ?? throw new ArgumentException($"Device type '{request.DeviceType}' not found.");

        var device = _mapper.Map<Entity.Device>(request);
        device.DeviceType = deviceType;

        device.Status = Domain.Enums.DeviceStatus.On;
        device.Brightness = (bool)request.IsDimmable! ? request.Brightness ?? 0 : 0;

        await _deviceRepository.CreateAsync(device);

        return device.Id;
    }
}
