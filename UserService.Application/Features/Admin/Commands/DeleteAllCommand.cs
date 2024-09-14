using MediatR;
using Entity = UserService.Domain.Entities;

namespace UserService.Application.Features.Admin.Commands;

public class DeleteAllCommand : IRequest
{
    public IEnumerable<Entity.User> Users { get; set; }

    public DeleteAllCommand(IEnumerable<Entity.User> users)
    {
        Users = users;
    }
}
