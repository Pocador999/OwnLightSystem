using MediatR;
using UserService.Application.DTOs;

namespace UserService.Application.Features.User.Queries;

public class GetByUserNameQuery(string userName) : IRequest<UserResponseDTO>
{
    public string UserName { get; set; } = userName;
}
