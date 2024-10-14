using AutoMapper;
using AutomationService.Application.Features.Group.Commands;
using AutomationService.Domain.Interfaces;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace AutomationService.Application.Features.Group.Handlers.Commands;

public class UpdateGroupCommandHandler(
    IGroupRepository groupRepository,
    IMapper mapper,
    IValidator<UpdateGroupCommand> validator
) : IRequestHandler<UpdateGroupCommand>
{
    private readonly IGroupRepository _groupRepository = groupRepository;
    private readonly IMapper _mapper = mapper;
    private readonly IValidator<UpdateGroupCommand> _validator = validator;

    public async Task<Unit> Handle(UpdateGroupCommand request, CancellationToken cancellationToken)
    {
        await _validator.ValidateAndThrowAsync(request, cancellationToken);
        var group =
            await _groupRepository.GetByIdAsync(request.Id)
            ?? throw new KeyNotFoundException("Grupo não encontrado.");

        var newName = await _groupRepository.GetUserGroupByNameAsync(
            group.UserId,
            request.Name,
            cancellationToken
        );
        if (newName == null)
        {
            _mapper.Map(request, group);
            await _groupRepository.UpdateAsync(group, cancellationToken);
            return Unit.Value;
        }
        else
            throw new InvalidOperationException("Já existe um grupo com esse nome.");
    }
}
