using System;
using MediatR;
using UserService.Application.Common.Messages;

namespace UserService.Application.Features.User.Commands;

public class DeleteCommand(Guid id) : IRequest<Messages>
{
    public Guid Id { get; set; } = id;
}
