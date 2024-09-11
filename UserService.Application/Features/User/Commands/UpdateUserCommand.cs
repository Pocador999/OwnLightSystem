using System.Text.Json.Serialization;
using MediatR;
using UserService.Application.DTOs;

namespace UserService.Application.Features.User.Commands;

public class UpdateUserCommand : IRequest<UserResponseDTO>
{
    [JsonIgnore]
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string UserName { get; set; }
}
