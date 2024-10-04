using AutoMapper;
using DeviceService.Application.DTOs;
using DeviceService.Application.Features.Device.Queries;
using DeviceService.Domain.Interfaces;
using MediatR;

namespace DeviceService.Application.Features.Device.Handlers.QueryHandlers;

public class GetHardwareResponseQueryHandler(IMapper mapper, IDeviceRepository deviceRepository)
    : IRequestHandler<GetHardwareResponseQuery, PaginatedResultDTO<HardwareResponseDTO>>
{
    private readonly IMapper _mapper = mapper;
    private readonly IDeviceRepository _deviceRepository = deviceRepository;

    public async Task<PaginatedResultDTO<HardwareResponseDTO>> Handle(
        GetHardwareResponseQuery request,
        CancellationToken cancellationToken
    )
    {
        // Validação se os IDs estão presentes
        if (request.DeviceIds == null || request.DeviceIds.Length == 0)
        {
            throw new ArgumentException("É necessário informar ao menos um id de dispositivo.");
        }

        // Busca os dispositivos por IDs
        var devices = await _deviceRepository.GetDevicesByIdsAsync(
            request.DeviceIds,
            request.PageNumber,
            request.PageSize
        );

        if (!devices.Any())
        {
            throw new KeyNotFoundException("Nenhum dispositivo encontrado para os IDs fornecidos.");
        }

        var devicesDTO = _mapper.Map<IEnumerable<HardwareResponseDTO>>(devices);

        return new PaginatedResultDTO<HardwareResponseDTO>(
            devicesDTO.Count(),
            request.PageNumber,
            request.PageSize,
            devicesDTO
        );
    }
}
