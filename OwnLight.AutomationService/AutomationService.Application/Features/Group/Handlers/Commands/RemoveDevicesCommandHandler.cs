using System.Text.Json;
using AutomationService.Application.Features.Group.Commands;
using AutomationService.Domain.Interfaces;
using MediatR;

namespace AutomationService.Application.Features.Group.Handlers.Commands;

public class RemoveDevicesCommandHandler(IGroupRepository groupRepository)
    : IRequestHandler<RemoveDevicesCommand, string>
{
    private readonly IGroupRepository _groupRepository = groupRepository;

    public async Task<string> Handle(
        RemoveDevicesCommand request,
        CancellationToken cancellationToken
    )
    {
        var group =
            await _groupRepository.GetByIdAsync(request.GroupId)
            ?? throw new KeyNotFoundException("Grupo não encontrado.");

        if (request.DeviceIds == null || request.DeviceIds.Length == 0)
            throw new ArgumentException("É necessário informar ao menos um dispositivo.");

        await _groupRepository.RemoveDevicesFromGroupAsync(
            group.Id,
            request.DeviceIds,
            cancellationToken
        );

        var response = group.DeviceIds?.Split(',').Select(Guid.Parse).ToArray();

        return JsonSerializer.Serialize(response);
    }
}
