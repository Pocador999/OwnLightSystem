using System;
using AutoMapper;
using DeviceService.Application.Features.DeviceType.Commands;
using DeviceService.Domain.Interfaces;
using MediatR;

namespace DeviceService.Application.Features.DeviceType.Handlers.CommandHandlers;

public class UpdateDeviceTypeCommandHandler(
    IDeviceTypeRepository deviceTypeRepository,
    IMapper mapper
) : IRequestHandler<UpdateDeviceTypeCommand>
{
    private readonly IDeviceTypeRepository _deviceTypeRepository = deviceTypeRepository;
    private readonly IMapper _mapper = mapper;

    public async Task<Unit> Handle(
        UpdateDeviceTypeCommand request,
        CancellationToken cancellationToken
    )
    {
        var deviceType =
            await _deviceTypeRepository.GetByIdAsync(request.Id)
            ?? throw new KeyNotFoundException($"DeviceType with ID {request.Id} not found.");

        _mapper.Map(request, deviceType);

        await _deviceTypeRepository.UpdateAsync(deviceType);

        return Unit.Value;
    }
}
