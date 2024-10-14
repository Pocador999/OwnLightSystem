using AutomationService.Application.Features.Group.Commands;
using AutomationService.Domain.Interfaces;
using MediatR;

namespace AutomationService.Application.Features.Group.Handlers.Commands;

public class DeleteGroupCommandHandler(IGroupRepository groupRepository)
    : IRequestHandler<DeleteGroupCommand>
{
    private readonly IGroupRepository _groupRepository = groupRepository;

    public async Task<Unit> Handle(DeleteGroupCommand request, CancellationToken cancellationToken)
    {
        var group =
            await _groupRepository.GetByIdAsync(request.Id)
            ?? throw new KeyNotFoundException("Grupo não encontrado.");

        await _groupRepository.DeleteAsync(group.Id, cancellationToken);
        return Unit.Value;
    }
}
