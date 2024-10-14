using AutomationService.Application.Features.Group.Commands;
using AutomationService.Domain.Interfaces;
using MediatR;

namespace AutomationService.Application.Features.Group.Handlers.Commands;

public class AddDevicesCommandHandler(IGroupRepository groupRepository)
    : IRequestHandler<AddDevicesCommand>
{
    private readonly IGroupRepository _groupRepository = groupRepository;

    public async Task<Unit> Handle(AddDevicesCommand request, CancellationToken cancellationToken)
    {
        var group =
            await _groupRepository.GetByIdAsync(request.GroupId)
            ?? throw new KeyNotFoundException("Grupo não encontrado.");

        if (request.DeviceIds == null || request.DeviceIds.Length == 0)
            throw new ArgumentException("É necessário informar ao menos um dispositivo.");

        await _groupRepository.AddDevicesToGroupAsync(
            group.Id,
            request.DeviceIds,
            cancellationToken
        );
        return Unit.Value;
    }
}
