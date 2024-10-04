using AutoMapper;
using DeviceService.Application.Features.DeviceType.Commands;
using DeviceService.Domain.Interfaces;
using MediatR;
using Type = DeviceService.Domain.Entities;

namespace DeviceService.Application.Features.DeviceType.Handlers.CommandHandlers;

public class CreateDeviceTypeCommandHandler(
    IDeviceTypeRepository deviceTypeRepository,
    IMapper mapper
) : IRequestHandler<CreateDeviceTypeCommand>
{
    private readonly IDeviceTypeRepository _deviceTypeRepository = deviceTypeRepository;
    private readonly IMapper _mapper = mapper;

    public async Task<Unit> Handle(
        CreateDeviceTypeCommand request,
        CancellationToken cancellationToken
    )
    {
        var deviceType = _mapper.Map<Type.DeviceType>(request);
        await _deviceTypeRepository.CreateAsync(deviceType);
        return Unit.Value;
    }
}
