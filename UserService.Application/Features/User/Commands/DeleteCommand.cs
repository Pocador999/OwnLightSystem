using System;
using MediatR;

namespace UserService.Application.Features.User.Commands;

public class DeleteCommand : IRequest<Unit>
{
    public Guid Id { get; set; }
}
