using AutoMapper;
using DeviceService.Application.DTOs;
using DeviceService.Application.Features.DeviceAction.Queries;
using DeviceService.Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace DeviceService.Application.Features.DeviceAction.Handlers;

public class GetActionsByDeviceIdQueryHandler
    : IRequestHandler<GetActionsByDeviceIdQuery, PaginatedResultDTO<ActionResponseDTO>>
{
    private readonly IDeviceActionRepository _deviceActionRepository;
    private readonly IDeviceRepository _deviceRepository;
    private readonly IMapper _mapper;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public GetActionsByDeviceIdQueryHandler(
        IDeviceActionRepository deviceActionRepository,
        IDeviceRepository deviceRepository,
        IMapper mapper,
        IHttpContextAccessor httpContextAccessor
    )
    {
        _deviceActionRepository = deviceActionRepository;
        _deviceRepository = deviceRepository;
        _mapper = mapper;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<PaginatedResultDTO<ActionResponseDTO>> Handle(
        GetActionsByDeviceIdQuery request,
        CancellationToken cancellationToken
    )
    {
        var userId = _httpContextAccessor.HttpContext?.Items["UserId"]?.ToString();
        if (string.IsNullOrEmpty(userId))
            throw new UnauthorizedAccessException("Usuário não autenticado.");

        var device =
            await _deviceRepository.GetByIdAsync(request.DeviceId)
            ?? throw new KeyNotFoundException("Dispositivo não encontrado.");

        var actions = await _deviceActionRepository.GetActionsByDeviceIdAsync(
            device.Id,
            request.PageNumber,
            request.PageSize
        );
        var actionsDTO = _mapper.Map<IEnumerable<ActionResponseDTO>>(actions);

        return new PaginatedResultDTO<ActionResponseDTO>(
            actionsDTO.Count(),
            request.PageNumber,
            request.PageSize,
            actionsDTO
        );
    }
}
