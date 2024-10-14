using MediatR;

namespace AutomationService.Application.Features.Group.Commands;

public class DeleteGroupCommand : IRequest
{
    public Guid Id { get; set; }
}
