using MediatR;
using UserService.Application.DTOs;

namespace UserService.Application.Features.User.Queries;

public class GetByIdQuery(Guid id) : IRequest<UserResponseDTO>
{
    public Guid Id { get; set; } = id;
}
